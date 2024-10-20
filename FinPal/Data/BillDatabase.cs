using FinPal.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinPal.Data
{
    public class BillDatabase : BaseDatabase<Bill>
    {
        public async Task<Bill> GetItemAsync(int id)
        {
            await Init();
            return await Database.Table<Bill>().Where(i => i.Id == id).FirstOrDefaultAsync();
        }

        public async Task<List<BillwithFC>> GetItemsAsyncWithFC()
        {
            var query = @"SELECT b.Name as FName, b.Note as FNote, c.Name as CName, c.Note as CNote, a.* FROM Bill a JOIN FinanceName b ON a.FinanceCode = b.Id JOIN Category c ON a.CategoryCode = c.Id";
            await Init();
            return await Database.QueryAsync<BillwithFC>(query);
        }
    }
}
