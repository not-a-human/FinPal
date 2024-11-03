using Microsoft.Extensions.Logging;
using FinPal.Data;
using FinPal.Services;
using CommunityToolkit.Maui;


namespace FinPal
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                // Initialize the .NET MAUI Community Toolkit
                .UseMauiCommunityToolkit()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("Poppins-ExtraLight.ttf", "PoppinsExtraLight");
                });

            // Delete the old database file if it exists
            //DatabaseHelper.DeleteDatabase("SinarSQLite.db3");

            builder.Services.AddMauiBlazorWebView();

            builder.Services.AddSingleton<SettingsDatabase>();
            builder.Services.AddSingleton<BillDatabase>();
            builder.Services.AddSingleton<CategoryDatabase>();
            builder.Services.AddSingleton<FinanceNameDatabase>();
            builder.Services.AddSingleton<SalaryDatabase>();
            builder.Services.AddSingleton<ExcelDataHelper>();

#if DEBUG
            builder.Services.AddBlazorWebViewDeveloperTools();
    		builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}
