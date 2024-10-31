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

        public async Task<List<BillwithFC>> GetItemsWithFCThisMonthAsync()
        {
            var query = @"SELECT 
                            b.Name AS FName, 
                            b.Note AS FNote, 
                            c.Name AS CName, 
                            c.Note AS CNote, 
                            a.* 
                        FROM Bill a 
                        JOIN FinanceName b ON a.FinanceCode = b.Id 
                        JOIN Category c ON a.CategoryCode = c.Id 
                        WHERE 
                            (datetime((StartDate / 10000000) - 62135596800, 'unixepoch') >= date('now', 'start of month') 
                             AND datetime((StartDate / 10000000) - 62135596800, 'unixepoch') < date('now', 'start of month', '+1 month'))
                        OR 
                            (datetime((EndDate / 10000000) - 62135596800, 'unixepoch') >= date('now', 'start of month') 
                             AND datetime((EndDate / 10000000) - 62135596800, 'unixepoch') < date('now', 'start of month', '+1 month'));";

            await Init();

            return await Database.QueryAsync<BillwithFC>(query);
        }

        public DateTime AddInterval(DateTime startDate, string frequency, int value)
        {
            switch (frequency.ToLower())
            {
                case "d":
                    return startDate.AddDays(value);

                case "w":
                    return startDate.AddDays(value * 7);

                case "b-w":
                    return startDate.AddDays(value * 14);

                case "m":
                    return startDate.AddMonths(value);

                case "b-m":
                    return startDate.AddMonths(value * 2);

                case "a":
                    return startDate.AddYears(value);

                default:
                    return startDate;
            }
        }

    }
}
