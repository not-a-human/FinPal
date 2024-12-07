using FinPal.Models;
using FinPal.Constant;
using SQLite;
using Microsoft.VisualBasic;
using static SQLite.SQLite3;
using System.Diagnostics;

namespace FinPal.Data
{
    public class CategoryDatabase : BaseDatabase<Category>
    {
        // Check if data exists and insert default data
        public async Task SeedDataAsync()
        {
            await Init();
            // Check if the database is empty
            if (await GetCountAsync() > 0)
            {
                return; // If not empty, do not seed
            }

            var categories = new List<Category>
            {
                new Category {Id= 1, Name = "Needs", Note = "Bills, food, and transportation", Percentage = 50},
                new Category {Id= 2, Name = "Wants", Note = "Dining out, shopping, and subscriptions", Percentage = 30},
                new Category {Id= 3, Name = "Savings", Note = "Emergency savings, investments, or paying off debt", Percentage = 20}
            };

            foreach (var item in categories)
            {
                var result = await SaveItemAsync(item);
                Debug.WriteLine($"Inserted {item.Name}, result: {result}");
            }
        }
        public override async Task<int> SaveItemAsync(Category item)
        {
            await Init();

            if (await GetItemAsync(item.Id) != null)
                return await Database.UpdateAsync(item);

            item.Id = await GetCountAsync() + 1;
            return await Database.InsertAsync(item);
            
        }
        public async Task<Category> GetItemAsync(int id)
        {
            await Init();
            return await Database.Table<Category>().Where(i => i.Id == id).FirstOrDefaultAsync();
        }

        public async Task<Category> GetItemAsync(string name)
        {
            await Init();
            return await Database.Table<Category>().Where(i => i.Name == name).FirstOrDefaultAsync();
        }

        public async Task<List<JSChartArray>> GetPlanChartAsync()
        {
            await Init();
            var query = @"SELECT Id, Name, Percentage FROM Category WHERE Active";

            var categories = await Database.QueryAsync<Category>(query);
            
            // Map to JSChartArray
            var chartData = categories.Select(c => new JSChartArray
            {
                Id = c.Id,
                Name = c?.Name ?? "",
                Value = c?.Percentage ?? 0
            }).ToList();

            return chartData;
        }

        public async Task<List<AllocatedFunds>> GetAllocatedFundsAsync()
        {
            await Init();

            int currentMonth = DateTime.Now.Month;
            int currentYear = DateTime.Now.Year;
            decimal salary = 0;

            var query = @"SELECT * FROM Salary WHERE Month = ? AND Year = ? AND Active = 1";
            var salaries = await Database.QueryAsync<Salary>(query, currentMonth, currentYear);
            
            if(salaries.Count != 0)
                salary = salaries?.FirstOrDefault()?.Amount ?? 0;

            var categories = await Database.Table<Category>().Where(i => i.Active).ToListAsync();

            var fund = categories.Select(c => new AllocatedFunds
            {
                Id = c.Id, 
                Name = c.Name,
                Note = c.Note,
                Percentage = c.Percentage,
                funds = FundCheck(salary,c),
             }).ToList();


            return fund;
        }

        private decimal FundCheck(decimal Salary, Category c)
        {
            // False = FixedAmount , True = Percentage
            if (c.FixedOrPerc)
                return Salary * (c.Percentage / 100);
            else
                return c.FixedAmount;
        }
    }
}
