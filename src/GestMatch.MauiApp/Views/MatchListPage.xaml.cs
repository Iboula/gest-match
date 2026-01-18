using GestMatch.MauiApp.ViewModels;

namespace GestMatch.MauiApp.Views;

public partial class MatchListPage : ContentPage
{
    private readonly MatchListViewModel _viewModel;

    public MatchListPage(MatchListViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = _viewModel = viewModel;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await _viewModel.InitializeAsync();
    }
}
