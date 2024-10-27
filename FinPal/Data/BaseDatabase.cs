using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FinPal.Constant;
using FinPal.Interfaces;

namespace FinPal.Data
{
    public abstract class BaseDatabase<T> where T : new()
    {
        protected SQLiteAsyncConnection Database;
        public BaseDatabase() { }

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
            return await Database.Table<T>().Where(item => ((IModel)item).Active).ToListAsync();
        }

        // virtual = to allow overriding
        public virtual async Task<int> SaveItemAsync(T item)
        {
            await Init();
            if (((IModel)item).Id != 0)
                return await Database.UpdateAsync(item);
            else
                return await Database.InsertAsync(item);
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
            await Database.ExecuteAsync($"DROP TABLE IF EXISTS {typeof(T).Name}");
            await Database.CreateTableAsync<T>();
        }
    }
}
