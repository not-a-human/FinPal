using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FinPal.Constant;
using FinPal.Interfaces;
using System.Diagnostics;

namespace FinPal.Data
{
    public abstract class BaseDatabase<T> where T : new()
    {
        protected SQLiteAsyncConnection Database;

        public async Task Init()
        {
            if (Database is not null)
                return;
         
            Database = new SQLiteAsyncConnection(Constants.DatabasePath, Constants.Flags);
            var result = await Database.CreateTableAsync<T>();
        }

        public async Task<List<T>> GetItemsAsync()
        {
            await Init();
            return await Database.Table<T>().ToListAsync();
        }

        public async Task<List<T>> GetActiveItemsAsync()
        {
            await Init();
            // Fetch all records from the table
            var allItems = await Database.Table<T>().ToListAsync();

            // Filter the results in memory
            var activeItems = allItems
                .Where(i => i is IModel model && model.Active)
                .ToList();

            return activeItems;
        }

        // virtual = to allow overriding
        public virtual async Task<int> SaveItemAsync(T item)
        {
            await Init();
            if(item != null)
            {
                if (((IModel)item).Id != 0)
                    return await Database.UpdateAsync(item);
                else
                    return await Database.InsertAsync(item);
            } 
            return 0;
        }

        public async Task<int> DeleteItemAsync(T item)
        {
            await Init();
            return await Database.DeleteAsync(item);
        }

        public async Task<int> GetCountAsync()
        {
            if (Database == null)
                return 0;
            else
            {
                // Query to get the total count of rows
                return await Database.Table<T>().CountAsync();
            }
        }

        public async void DropTable()
        {
            await Init();
            await Database.ExecuteAsync($"DROP TABLE IF EXISTS {typeof(T).Name}");
            Debug.WriteLine("Table " + typeof(T).Name + " dropped");
            await Database.CreateTableAsync<T>();
        }

        public async Task<int> ToggleItemAsync(T item)
        {
            await Init();

            if (item != null)
            {
                ((IModel)item).Active = !((IModel)item).Active;

                return await Database.UpdateAsync(item);
            }

            return 0;
        }
    }
}
