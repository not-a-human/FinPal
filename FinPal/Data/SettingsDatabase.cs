using FinPal.Models;
using FinPal.Constant;
using SQLite;
using static SQLite.SQLite3;
using System.Diagnostics;

namespace FinPal.Data
{
    public class SettingsDatabase
    {

        protected SQLiteAsyncConnection Database;
        public SettingsDatabase() { }
        public async Task Init()
        {
            if (Database is not null)
                return;

            Database = new SQLiteAsyncConnection(Constants.DatabasePath, Constants.Flags);
            var result = await Database.CreateTableAsync<Settings>();
        }

        // Check if data exists and insert default data
        public async Task SeedDataAsync()
        {
            await Init();

            if (await Database.Table<Settings>().CountAsync() > 0)
            {
                return; // If not empty, do not seed
            }

            var settings = new List<Settings>
            {
                new Settings {SetKey = "UIMode", SetStr = "dark"},
            };

            foreach (var item in settings)
            {
                await Database.InsertAsync(item);
            }
        }

        public async Task<Settings> GetItemAsync(string setKey)
        {
            await Init();
            var result = await Database.Table<Settings>().Where(i => i.SetKey == setKey).FirstOrDefaultAsync();
            if(result == null)
            {
                return null;
            }
            return result;
        }

        public virtual async Task<int> UpdateSettingAsync(Settings item)
        {
            return await Database.UpdateAsync(item);
            
        }

        public async Task<int> ChangeUIMode(Settings item)
        {
            return await Database.UpdateAsync(item);
        }
    }
}
