using FinPal.Data;

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

            MainPage = new MainPage();

            }

        private async void SeedDatabase()
        {
            await CDatabase.SeedDataAsync();
            await FDatabase.SeedDataAsync();
            await SDatabase.SeedDataAsync();
            await SetDatabase.SeedDataAsync();
        }
    }
}
