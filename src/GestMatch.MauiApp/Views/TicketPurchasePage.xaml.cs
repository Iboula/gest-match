using GestMatch.MauiApp.ViewModels;

namespace GestMatch.MauiApp.Views;

public partial class TicketPurchasePage : ContentPage
{
    public TicketPurchasePage(TicketPurchaseViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}
