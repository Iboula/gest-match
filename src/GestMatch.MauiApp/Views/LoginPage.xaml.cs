using GestMatch.MauiApp.ViewModels;

namespace GestMatch.MauiApp.Views;

public partial class LoginPage : ContentPage
{
    public LoginPage(LoginViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}
