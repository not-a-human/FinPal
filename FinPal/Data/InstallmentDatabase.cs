using FinPal.Models;
using FinPal.Constant;
using SQLite;

namespace FinPal.Data
{
    public class InstallmentDatabase : BaseDatabase<Installment>
    {
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
    }
}
