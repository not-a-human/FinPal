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
            var query = @"
            
                WITH BillDates AS (
                    SELECT 
                        b.Name AS FName, 
                        b.Note AS FNote, 
                        c.Name AS CName, 
                        c.Note AS CNote, 
                        a.*, 
                        datetime((a.StartDate / 10000000) - 62135596800, 'unixepoch') AS StartDateConverted,
                        datetime((a.EndDate / 10000000) - 62135596800, 'unixepoch') AS EndDateConverted
                    FROM Bill a
                    JOIN FinanceName b ON a.FinanceCode = b.Id
                    JOIN Category c ON a.CategoryCode = c.Id
                )
                SELECT *
                FROM BillDates
                WHERE 
                     -- Condition 1: Start or end date is within the current month
                    (StartDateConverted BETWEEN date('now', 'start of month') AND date('now', 'start of month', '+1 month', '-1 day'))
                    OR 
                    (EndDateConverted BETWEEN date('now', 'start of month') AND date('now', 'start of month', '+1 month', '-1 day'))
                    OR
                    -- Condition 2: Payment spans the entire month (start date is before and end date is after the month)
                    (StartDateConverted < date('now', 'start of month') 
                     AND EndDateConverted > date('now', 'start of month', '+1 month', '-1 day'));


            ";

            await Init();

            return await Database.QueryAsync<BillwithFC>(query);
        }

        public override async Task<int> SaveItemAsync(Bill item)
        {
            await Init();

            if (await GetItemAsync(item.Id) != null)
                return await Database.UpdateAsync(item);

            item.Id = await GetCountAsync() + 1;
            return await Database.InsertAsync(item);

        }

        public DateTime AddInterval(DateTime startDate, int value)
        {
            value = value - 1;
            return startDate.AddMonths(value);
        }

        public string intervalSting(string frequency)
        {
            switch (frequency.ToLower())
            {
                case "d":
                    return "Daily";

                case "w":
                    return "Weekly";

                case "b-w":
                    return "Bi-Weekly";

                case "m":
                    return "Monthly";

                case "a":
                    return "Anually";

                default:
                    return "None";
            }
        }

    }
}
