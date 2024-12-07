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
                new Settings {SetKey = "UIMode", SetStr = "dark", APname = "Theme"},
                new Settings {SetKey = "--primary-color", SetStr = GetDefaultValue("--primary-color", false), APname = "CSSColor"},
                new Settings {SetKey = "--warning-color", SetStr = GetDefaultValue("--warning-color", false), APname = "CSSColor"},
                new Settings {SetKey = "--success-color", SetStr = GetDefaultValue("--success-color", false), APname = "CSSColor"},
                new Settings {SetKey = "--info-color", SetStr = GetDefaultValue("--info-color", false), APname = "CSSColor"},
                new Settings {SetKey = "--danger-color", SetStr = GetDefaultValue("--danger-color", false), APname = "CSSColor"},
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
                if(GetDefaultValue(setKey, false) != "")
                {
                    Settings item = new Settings { SetKey = setKey, SetStr = GetDefaultValue(setKey, false), APname = GetDefaultValue(setKey, true) };
                    await Database.InsertAsync(item);
                    return item;
                }
                
                return null;
            }

            if (string.IsNullOrEmpty(result.APname))
            {
                result.APname = GetDefaultValue(setKey, true);
                await UpdateSettingAsync(result);
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

        public async Task<List<Settings>> GetColorListAsync()
        {
            await Init();

            return await Database.Table<Settings>().Where(i => i.APname == "CSSColor").ToListAsync();
        }

        /// <summary>
        /// Return APName or SetStr
        /// </summary>
        /// <param name="APorStr">True = AP, False = Str</param>
        public string GetDefaultValue(string SetKey, bool APorStr)
        {
            switch (SetKey)
            {
                case "UIMode":
                    return GetApNameOrSetStr("dark","Theme",APorStr);
                case "--primary-color":
                    return GetApNameOrSetStr(FinPal.Constant.Constants.PrimaryColor, "CSSColor", APorStr);
                case "--warning-color": 
                    return GetApNameOrSetStr(FinPal.Constant.Constants.WarningColor, "CSSColor", APorStr);
                case "--success-color": 
                    return GetApNameOrSetStr(FinPal.Constant.Constants.SuccessColor, "CSSColor", APorStr);
                case "--info-color":
                    return GetApNameOrSetStr(FinPal.Constant.Constants.InfoColor, "CSSColor", APorStr);
                case "--danger-color": 
                    return GetApNameOrSetStr(FinPal.Constant.Constants.DangerColor, "CSSColor", APorStr);
                default:
                    return "";
            }
        }

        /// <summary>
        /// Return APName or SetStr
        /// </summary>
        /// <param name="APorStr">True = AP, False = Str</param>
        private string GetApNameOrSetStr(string SetStr, string APName, bool APorStr)
        {
            if (APorStr)
            {
                return APName;
            }
            return SetStr;
        }

        public string GetColorCaption(string SetKey)
        {
            switch (SetKey)
            {
                case "--primary-color":
                    return "Primary";
                case "--warning-color":
                    return "Warning";
                case "--success-color":
                    return "Success";
                case "--info-color":
                    return "Info";
                case "--danger-color":
                    return "Danger";
                default:
                    return "";
            }
        }
    }
}
