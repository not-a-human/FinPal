using Microsoft.Extensions.Logging;
using FinPal.Data;
using FinPal.Services;


namespace FinPal
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("Poppins-ExtraLight.ttf", "PoppinsExtraLight");
                });

            // Delete the old database file if it exists
            //DatabaseHelper.DeleteDatabase("SinarSQLite.db3");

            builder.Services.AddMauiBlazorWebView(); 
            builder.Services.AddBlazorBootstrap();

            builder.Services.AddSingleton<SettingsDatabase>();
            builder.Services.AddSingleton<InstallmentDatabase>();
            builder.Services.AddSingleton<BillDatabase>();
            builder.Services.AddSingleton<CategoryDatabase>();
            builder.Services.AddSingleton<FinanceNameDatabase>();
            builder.Services.AddSingleton<SalaryDatabase>();

#if DEBUG
            builder.Services.AddBlazorWebViewDeveloperTools();
    		builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}
