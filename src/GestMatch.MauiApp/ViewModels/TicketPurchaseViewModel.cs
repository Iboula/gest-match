using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using GestMatch.MauiApp.Services;

namespace GestMatch.MauiApp.ViewModels;

/// <summary>
/// ViewModel pour l'achat de billet
/// </summary>
[QueryProperty(nameof(MatchId), "MatchId")]
[QueryProperty(nameof(MatchName), "MatchName")]
[QueryProperty(nameof(Price), "Price")]
public partial class TicketPurchaseViewModel : ObservableObject
{
    private readonly ITicketService _ticketService;
    private readonly INavigationService _navigationService;

    [ObservableProperty]
    private Guid _matchId;

    [ObservableProperty]
    private string _matchName = string.Empty;

    [ObservableProperty]
    private decimal _price;

    [ObservableProperty]
    private string _phoneNumber = string.Empty;

    [ObservableProperty]
    private string _selectedPaymentMethod = "Wave";

    [ObservableProperty]
    private bool _isLoading;

    public List<string> PaymentMethods { get; } = new() { "Wave", "Orange Money", "Free Money" };

    public TicketPurchaseViewModel(ITicketService ticketService, INavigationService navigationService)
    {
        _ticketService = ticketService;
        _navigationService = navigationService;
    }

    [RelayCommand]
    private async Task PurchaseAsync()
    {
        if (IsLoading) return;

        if (string.IsNullOrWhiteSpace(PhoneNumber))
        {
            await Application.Current!.MainPage!.DisplayAlert("Erreur", "Veuillez entrer votre numéro de téléphone", "OK");
            return;
        }

        IsLoading = true;

        try
        {
            var ticket = await _ticketService.PurchaseTicketAsync(MatchId, Price);
            
            if (ticket != null)
            {
                await Application.Current!.MainPage!.DisplayAlert("Succès", "Billet acheté avec succès!", "OK");
                await _navigationService.NavigateToAsync("//tickets");
            }
            else
            {
                await Application.Current!.MainPage!.DisplayAlert("Erreur", "Échec de l'achat du billet", "OK");
            }
        }
        catch (Exception ex)
        {
            await Application.Current!.MainPage!.DisplayAlert("Erreur", ex.Message, "OK");
        }
        finally
        {
            IsLoading = false;
        }
    }

    [RelayCommand]
    private async Task CancelAsync()
    {
        await _navigationService.GoBackAsync();
    }
}
