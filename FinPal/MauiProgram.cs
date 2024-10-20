using Microsoft.Extensions.Logging;
using SinarFinance.Data;
using SinarFinance.Services;

namespace SinarFinance
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


            // Specify your database name
            string dbName = "SinarSQLite.db3";

            // Delete the old database file if it exists
            DatabaseHelper.DeleteDatabase(dbName);

            builder.Services.AddMauiBlazorWebView();


            builder.Services.AddSingleton<InstallmentDatabase>();
            builder.Services.AddSingleton<FinanceNameDatabase>();

#if DEBUG
            builder.Services.AddBlazorWebViewDeveloperTools();
    		builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}
