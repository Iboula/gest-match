using GestMatch.MauiApp.ViewModels;

namespace GestMatch.MauiApp.Views;

public partial class MatchDetailPage : ContentPage
{
    public MatchDetailPage(MatchDetailViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}
