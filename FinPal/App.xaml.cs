using FinPal.Data;

namespace FinPal
{
    public partial class App : Application
    {
        public static CategoryDatabase CDatabase { get; private set; }
        public static FinanceNameDatabase FDatabase { get; private set; }
        public static SettingsDatabase SDatabase { get; private set; }
        public App()
        {
            InitializeComponent();

            CDatabase = new CategoryDatabase();
            FDatabase = new FinanceNameDatabase(CDatabase);
            SDatabase = new SettingsDatabase();

            SeedDatabase();

            MainPage = new MainPage();
        }

        private async void SeedDatabase()
        {
            await CDatabase.SeedDataAsync();
            await FDatabase.SeedDataAsync();
            await SDatabase.SeedDataAsync();
        }
    }
}
