using FinPal.Models;
using FinPal.Constant;
using SQLite;
using static SQLite.SQLite3;
using System.Diagnostics;
using Microsoft.AspNetCore.Components;

namespace FinPal.Data
{
    public class FinanceNameDatabase : BaseDatabase<FinanceName>
    {

        private readonly CategoryDatabase cDatabase;

        public FinanceNameDatabase(CategoryDatabase categoryDatabase)
        {
            cDatabase = categoryDatabase;
        }

        // Check if data exists and insert default data
        public async Task SeedDataAsync()
        {
            await Init();
            // Check if the database is empty
            if (await GetCountAsync() > 0)
            {
                return; // If not empty, do not seed
            }

            Category cItem =  await cDatabase.GetItemAsync("Financial Services/Loans");
            Category cItem2 =  await cDatabase.GetItemAsync("Bills & Utilities");

            var financename = new List<FinanceName>
            {
                new FinanceName {Name = "Maybank Credit Card", CategoryId = cItem.Id },
                new FinanceName {Name = "ShopeePayLater", CategoryId = cItem.Id},
                new FinanceName {Name = "Maxis", CategoryId = cItem2.Id}
            };

            foreach (var item in financename)
            {
                var result = await SaveItemAsync(item);
                Debug.WriteLine($"Inserted {item.Name}, result: {result}");
            }
        }
        
        public async Task<FinanceName> GetItemAsync(int id)
        {
            await Init();
            return await Database.Table<FinanceName>().Where(i => i.Id == id).FirstOrDefaultAsync();
        }
    }
}
