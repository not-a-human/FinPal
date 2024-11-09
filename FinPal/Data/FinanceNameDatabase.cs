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
        // Check if data exists and insert default data
        public async Task SeedDataAsync()
        {
            await Init();
            // Check if the database is empty
            if (await GetCountAsync() > 0)
            {
                return; // If not empty, do not seed
            }

            var financename = new List<FinanceName>
            {
                new FinanceName {Name = "Credit Card", CategoryId = 3 },
                new FinanceName {Name = "ShopeePayLater", CategoryId = 2 },
                new FinanceName {Name = "Phone Bill", CategoryId = 1 }
            };

            foreach (var item in financename)
            {
                var result = await SaveItemAsync(item);
                Debug.WriteLine($"Inserted {item.Name}, result: {result}");
            }
        }

        public override async Task<int> SaveItemAsync(FinanceName item)
        {
            await Init();

            if (await GetItemAsync(item.Id) != null)
                return await Database.UpdateAsync(item);

            item.Id = await GetCountAsync() + 1;
            return await Database.InsertAsync(item);

        }

        public async Task<FinanceName> GetItemAsync(int id)
        {
            await Init();
            return await Database.Table<FinanceName>().Where(i => i.Id == id).FirstOrDefaultAsync();
        }
    }
}
