using FinPal.Data;
using FinPal.Models;

namespace FinPal
{
    public partial class App : Application
    {
        public static CategoryDatabase CDatabase { get; private set; } = new CategoryDatabase();
        public static FinanceNameDatabase FDatabase { get; private set; } = new FinanceNameDatabase();
        public static SettingsDatabase SetDatabase { get; private set; } = new SettingsDatabase();
        public static SalaryDatabase SDatabase { get; private set; } = new SalaryDatabase();
        public App()
        {
            InitializeComponent();

            SeedDatabase();

            EnsureColumnExistsAsync("Settings", "APname", "VARCHAR");

            MainPage = new MainPage();

            }

        private async void SeedDatabase()
        {
            await CDatabase.SeedDataAsync();
            await FDatabase.SeedDataAsync();
            await SDatabase.SeedDataAsync();
            await SetDatabase.SeedDataAsync();
        }

        private async void EnsureColumnExistsAsync(string tableName, string columnName, string columnDefinition)
        {
            SQLite.SQLiteAsyncConnection Database = new SQLite.SQLiteAsyncConnection(FinPal.Constant.Constants.DatabasePath, FinPal.Constant.Constants.Flags);

            // Check if the column exists in the table
            var query = $"PRAGMA table_info({tableName})";
            var columns = await Database.QueryAsync<TableColumnInfo>(query);

            // If the column is not found, alter the table to add it
            if (!columns.Any(c => c.Name.Equals(columnName, StringComparison.OrdinalIgnoreCase)))
            {
                var alterTableQuery = $"ALTER TABLE {tableName} ADD COLUMN {columnName} {columnDefinition}";
                await Database.ExecuteAsync(alterTableQuery);
                Console.WriteLine($"Added column '{columnName}' to table '{tableName}'.");
            }
            else
            {
                Console.WriteLine($"Column '{columnName}' already exists in table '{tableName}'.");
            }
        }
    }
}
