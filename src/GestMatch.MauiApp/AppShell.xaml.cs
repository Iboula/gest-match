using GestMatch.MauiApp.Views;

namespace GestMatch.MauiApp;

public partial class AppShell : Shell
{
    public AppShell()
    {
        InitializeComponent();

        // Enregistrement des routes
        Routing.RegisterRoute(nameof(LoginPage), typeof(LoginPage));
        Routing.RegisterRoute(nameof(MatchListPage), typeof(MatchListPage));
        Routing.RegisterRoute(nameof(MatchDetailPage), typeof(MatchDetailPage));
        Routing.RegisterRoute(nameof(TicketPurchasePage), typeof(TicketPurchasePage));
        Routing.RegisterRoute(nameof(MyTicketsPage), typeof(MyTicketsPage));
        Routing.RegisterRoute(nameof(TicketDetailPage), typeof(TicketDetailPage));
    }
}
