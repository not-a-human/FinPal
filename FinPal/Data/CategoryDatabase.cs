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
                new Category {Name = "Bills & Utilities", Note = "Telco bill, electric bills etc", Percentage = 5},
                new Category {Name = "Financial Services/Loans", Note = "Credit Card, loans etc", Percentage = 30},
                new Category {Name = "Investment", Note = "Saving for the future.", Percentage = 20},
                new Category {Name = "Sinking Fund", Note = "For emergency, 1st layer.", Percentage = 5},
                new Category {Name = "Emergency Fund", Note = "For emergency, 2nd layer.", Percentage = 10},
                new Category {Name = "Personal", Note = "Eat, Fuel etc", Percentage = 20},
                new Category {Name = "Household", Note = "Household funds.", Percentage = 10}
            };

            foreach (var item in categories)
            {
                var result = await SaveItemAsync(item);
                Debug.WriteLine($"Inserted {item.Name}, result: {result}");
            }
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
            var query = @"SELECT Name, Percentage FROM Category";

            var categories = await Database.QueryAsync<Category>(query);
            
            // Map to JSChartArray
            var chartData = categories.Select(c => new JSChartArray
            {
                Name = c.Name,
                Value = (decimal)c.Percentage
            }).ToList();

            return chartData;
        }

        public async Task<List<AllocatedFunds>> GetAllocatedFundsAsync()
        {
            await Init();

            int currentMonth = DateTime.Now.Month;
            int currentYear = DateTime.Now.Year;

            var query = @"SELECT * FROM Salary WHERE Month = ? AND Year = ? AND Active = 1";
            var salaries = await Database.QueryAsync<Salary>(query, currentMonth, currentYear);

            var categories = await Database.Table<Category>().ToListAsync();

            var fund = categories.Select(c => new AllocatedFunds
            {
                Id = c.Id, 
                Name = c.Name,
                Note = c.Note,
                Percentage = c.Percentage,
                funds = salaries.FirstOrDefault().Amount * (c.Percentage / 100),
             }).ToList();


            return fund;
        }
    }
}
