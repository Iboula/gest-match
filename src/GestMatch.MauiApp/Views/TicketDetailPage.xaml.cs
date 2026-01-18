using GestMatch.MauiApp.ViewModels;

namespace GestMatch.MauiApp.Views;

public partial class TicketDetailPage : ContentPage
{
    public TicketDetailPage(TicketDetailViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}
