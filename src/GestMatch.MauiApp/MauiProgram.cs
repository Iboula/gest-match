using GestMatch.MauiApp.Services;
using GestMatch.MauiApp.ViewModels;
using GestMatch.MauiApp.Views;
using Microsoft.Extensions.Logging;
using ZXing.Net.Maui.Controls;

namespace GestMatch.MauiApp;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        
        builder
            .UseMauiApp<App>()
            .UseBarcodeReader() // Pour scanner les QR Codes
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
            });

#if DEBUG
        builder.Logging.AddDebug();
#endif

        // Configuration des services
        RegisterServices(builder.Services);
        
        // Configuration des ViewModels
        RegisterViewModels(builder.Services);
        
        // Configuration des Views
        RegisterViews(builder.Services);

        return builder.Build();
    }

    private static void RegisterServices(IServiceCollection services)
    {
        // HTTP Client pour l'API
        services.AddHttpClient("GestMatchApi", client =>
        {
            client.BaseAddress = new Uri("http://localhost:5000"); // À configurer selon l'environnement
            client.Timeout = TimeSpan.FromSeconds(30);
        });

        services.AddSingleton<IAuthService, AuthService>();
        services.AddSingleton<IApiService, ApiService>();
        services.AddSingleton<IMatchService, MatchService>();
        services.AddSingleton<ITicketService, TicketService>();
        services.AddSingleton<INavigationService, NavigationService>();
    }

    private static void RegisterViewModels(IServiceCollection services)
    {
        services.AddTransient<LoginViewModel>();
        services.AddTransient<MatchListViewModel>();
        services.AddTransient<MatchDetailViewModel>();
        services.AddTransient<TicketPurchaseViewModel>();
        services.AddTransient<MyTicketsViewModel>();
        services.AddTransient<TicketDetailViewModel>();
    }

    private static void RegisterViews(IServiceCollection services)
    {
        services.AddTransient<LoginPage>();
        services.AddTransient<MatchListPage>();
        services.AddTransient<MatchDetailPage>();
        services.AddTransient<TicketPurchasePage>();
        services.AddTransient<MyTicketsPage>();
        services.AddTransient<TicketDetailPage>();
    }
}
