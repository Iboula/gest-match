using GestMatch.MauiApp.ViewModels;

namespace GestMatch.MauiApp.Views;

public partial class MyTicketsPage : ContentPage
{
    private readonly MyTicketsViewModel _viewModel;

    public MyTicketsPage(MyTicketsViewModel viewModel)
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
