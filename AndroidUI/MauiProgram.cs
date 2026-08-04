using Microsoft.Extensions.Logging;
using AndroidUI.Data;
using AndroidUI.Services;
namespace AndroidUI
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
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                });
            
            builder.Services.AddMauiBlazorWebView();

            builder.Services.AddSingleton<FinanceDatabase>();
            builder.Services.AddSingleton<AccountService>();
            builder.Services.AddSingleton<CategoryService>();
            builder.Services.AddSingleton<TransactionService>();
            builder.Services.AddSingleton<BudgetService>();
            builder.Services.AddSingleton<ReportService>();
            builder.Services.AddSingleton<ExportService>();
            builder.Services.AddSingleton<TransferService>();
            builder.Services.AddSingleton<IconService>();
            builder.Services.AddSingleton<ColorService>();
            builder.Services.AddSingleton<ShareService>();
            builder.Services.AddSingleton<AccountTypeService>();

#if DEBUG
            builder.Services.AddBlazorWebViewDeveloperTools();
            builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}
