using FinPal.Models;
using FinPal.Constant;
using SQLite;
using static SQLite.SQLite3;
using System.Diagnostics;

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
                new FinanceName {Name = "Maybank Credit Card"},
                new FinanceName {Name = "ShopeePayLater"}
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
