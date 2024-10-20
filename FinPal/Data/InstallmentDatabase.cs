using FinPal.Models;
using FinPal.Constant;
using SQLite;

namespace FinPal.Data
{
    public class InstallmentDatabase
    {
        SQLiteAsyncConnection Database;

        public InstallmentDatabase()
        {
        }

        async Task Init()
        {
            if (Database is not null)
                return;

            Database = new SQLiteAsyncConnection(Constants.DatabasePath, Constants.Flags);
            var result = await Database.CreateTableAsync<Installment>();
        }

        public async Task<List<Installment>> GetItemsAsync()
        {
            await Init();
            return await Database.Table<Installment>().ToListAsync();
        }

        public async Task<List<InstallmentwithFinance>> GetItemsAsyncWithFinance()
        {
            var query = @"SELECT b.Name as fName, b.Note as fNote, a.* FROM Installment a JOIN FinanceName b ON a.FinanceCode = b.Id";
            await Init();
            return await Database.QueryAsync<InstallmentwithFinance>(query);
        }

        public async Task<Installment> GetItemAsync(int id)
        {
            await Init();
            return await Database.Table<Installment>().Where(i => i.Id == id).FirstOrDefaultAsync();
        }

        public async Task<int> SaveItemAsync(Installment item)
        {
            await Init();
            if (item.Id != 0)
                return await Database.UpdateAsync(item);
            else
                return await Database.InsertAsync(item);
        }

        public async Task<int> DeleteItemAsync(Installment item)
        {
            await Init();
            return await Database.DeleteAsync(item);
        }
    }
}
