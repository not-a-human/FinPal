using FinPal.Models;
using FinPal.Constant;
using SQLite;

namespace FinPal.Data
{
    public class FinanceNameDatabase
    {
        SQLiteAsyncConnection Database;

        public FinanceNameDatabase()
        {
        }

        async Task Init()
        {
            if (Database is not null)
                return;

            Database = new SQLiteAsyncConnection(Constants.DatabasePath, Constants.Flags);
            var result = await Database.CreateTableAsync<FinanceName>();
        }

        // Check if data exists and insert default data
        private async Task SeedDataAsync()
        {
            var financeName = await Database.Table<FinanceName>().ToListAsync();
            if (financeName.Count == 0)
            {
                // Insert default records if no records exist
                await Database.InsertAsync(new FinanceName { Name = "Administrator" });
            }
        }
        public async Task<int> GetCountAsync()
        {
            if (Database == null)
                return 0;
            else
            {
                // Query to get the total count of rows
                return await Database.Table<FinanceName>().CountAsync();
            }
        }
        public async Task<List<FinanceName>> GetItemsAsync()
        {
            await Init();
            return await Database.Table<FinanceName>().ToListAsync();
        }

        public async Task<FinanceName> GetItemAsync(int id)
        {
            await Init();
            return await Database.Table<FinanceName>().Where(i => i.Id == id).FirstOrDefaultAsync();
        }

        public async Task<int> SaveItemAsync(FinanceName item)
        {
            await Init();
            if (item.Id != 0)
                return await Database.UpdateAsync(item);
            else
                return await Database.InsertAsync(item);
        }

        public async Task<int> DeleteItemAsync(FinanceName item)
        {
           
                await Init();
                return await Database.DeleteAsync(item);
                
        }
    }

}
