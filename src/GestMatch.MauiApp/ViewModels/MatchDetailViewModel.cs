using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using GestMatch.MauiApp.Models;
using GestMatch.MauiApp.Services;

namespace GestMatch.MauiApp.ViewModels;

/// <summary>
/// ViewModel pour les détails d'un match
/// </summary>
[QueryProperty(nameof(MatchId), "MatchId")]
public partial class MatchDetailViewModel : ObservableObject
{
    private readonly IMatchService _matchService;
    private readonly INavigationService _navigationService;

    [ObservableProperty]
    private Guid _matchId;

    [ObservableProperty]
    private MatchModel? _match;

    [ObservableProperty]
    private bool _isLoading;

    public MatchDetailViewModel(IMatchService matchService, INavigationService navigationService)
    {
        _matchService = matchService;
        _navigationService = navigationService;
    }

    partial void OnMatchIdChanged(Guid value)
    {
        _ = LoadMatchAsync();
    }

    private async Task LoadMatchAsync()
    {
        if (MatchId == Guid.Empty) return;

        IsLoading = true;

        try
        {
            Match = await _matchService.GetMatchByIdAsync(MatchId);
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
    private async Task BuyTicketAsync()
    {
        if (Match == null) return;

        await _navigationService.NavigateToAsync("ticketpurchase", new Dictionary<string, object>
        {
            { "MatchId", Match.Id },
            { "MatchName", Match.DisplayName },
            { "Price", Match.StandardTicketPrice }
        });
    }
}
