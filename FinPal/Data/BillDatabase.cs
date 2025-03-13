using DocumentFormat.OpenXml.Office2010.Excel;
using FinPal.Models;
using SQLite;
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

        public async Task<List<BillwithFC>> GetItemsWithFCThisMonthAsync(int year, int month)
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
                    WHERE c.Active AND b.Active
                )
                SELECT *
                FROM BillDates
                WHERE 
                     -- Condition 1: Start or end date is within the specified month
                    (StartDateConverted BETWEEN date(?1, ?2) AND date(?1, ?2, '+1 month', '-1 day'))
                    OR 
                    (EndDateConverted BETWEEN date(?1, ?2) AND date(?1, ?2, '+1 month', '-1 day'))
                    OR
                    -- Condition 2: Payment spans the entire specified month
                    (StartDateConverted < date(?1, ?2) 
                     AND EndDateConverted > date(?1, ?2, '+1 month', '-1 day'))
                    OR
                    (Continuous);
            ";

            // Convert year and month into a date string (e.g., "2024-11-01").
            string startOfMonth = $"{year:D4}-{month:D2}-01";

            await Init();

            return await Database.QueryAsync<BillwithFC>(query, startOfMonth, "start of month");
        }

        public async Task<List<(string? CategoryCode, decimal Amount)>> GetAmountByCategoryAsync(int year, int month)
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
                    WHERE c.Active AND b.Active
                )
                SELECT CategoryCode, SUM(AmountDue) AS Amount
                FROM BillDates
                WHERE 
                     -- Condition 1: Start or end date is within the specified month
                    (StartDateConverted BETWEEN date(?1, ?2) AND date(?1, ?2, '+1 month', '-1 day'))
                    OR 
                    (EndDateConverted BETWEEN date(?1, ?2) AND date(?1, ?2, '+1 month', '-1 day'))
                    OR
                    -- Condition 2: Payment spans the entire specified month
                    (StartDateConverted < date(?1, ?2) 
                     AND EndDateConverted > date(?1, ?2, '+1 month', '-1 day'))
                    OR
                    (Continuous)
                GROUP BY CategoryCode;
            ";

            // Convert year and month into a date string (e.g., "2024-11-01").
            string startOfMonth = $"{year:D4}-{month:D2}-01";

            await Init();

            return await Database.QueryAsync<(string? CategoryCode, decimal Amount)> (query, startOfMonth, "start of month");
        }

        public override async Task<int> SaveItemAsync(Bill item)
        {
            await Init();

            if (item.Total == 0)
                item.Total = item.AmountDue * item.Period;

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

        public async Task<int> GetActiveCountAsync(int year, int month)
        {
            await Init();
            if (Database == null)
                return 0;
            else
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
                    WHERE c.Active AND b.Active
                )
                SELECT count(id)
                FROM BillDates
                WHERE 
                     -- Condition 1: Start or end date is within the specified month
                    (StartDateConverted BETWEEN date(?1, ?2) AND date(?1, ?2, '+1 month', '-1 day'))
                    OR 
                    (EndDateConverted BETWEEN date(?1, ?2) AND date(?1, ?2, '+1 month', '-1 day'))
                    OR
                    -- Condition 2: Payment spans the entire specified month
                    (StartDateConverted < date(?1, ?2) 
                     AND EndDateConverted > date(?1, ?2, '+1 month', '-1 day'))
                    OR
                    (Continuous);
            ";

                // Convert year and month into a date string (e.g., "2024-11-01").
                string startOfMonth = $"{year:D4}-{month:D2}-01";

                await Init();
                return await Database.ExecuteScalarAsync<int>(query, startOfMonth, "start of month");
            }
        }

        public async Task<List<int>> GetActiveFinanceID()
        {
            await Init();

            var activeCategories = await Database.Table<Category>()
                                          .Where(c => c.Active)
                                          .ToListAsync();

            var activeCategoryIds = activeCategories.Select(c => c.Id).ToList();

            var activeFinanceNames = await Database.Table<FinanceName>()
                                            .Where(b => b.Active && activeCategoryIds.Contains(b.CategoryId))
                                            .ToListAsync();

            var activeFinanceIds = activeFinanceNames.Select(b => b.Id).ToList();

            return activeFinanceIds;
        }

    }
}
