using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using GestMatch.MauiApp.Models;
using GestMatch.MauiApp.Services;
using System.Collections.ObjectModel;

namespace GestMatch.MauiApp.ViewModels;

/// <summary>
/// ViewModel pour la liste des matchs
/// </summary>
public partial class MatchListViewModel : ObservableObject
{
    private readonly IMatchService _matchService;
    private readonly INavigationService _navigationService;

    [ObservableProperty]
    private ObservableCollection<MatchModel> _matches = new();

    [ObservableProperty]
    private bool _isLoading;

    [ObservableProperty]
    private bool _isRefreshing;

    [ObservableProperty]
    private string? _searchCity;

    public MatchListViewModel(IMatchService matchService, INavigationService navigationService)
    {
        _matchService = matchService;
        _navigationService = navigationService;
    }

    public async Task InitializeAsync()
    {
        await LoadMatchesAsync();
    }

    [RelayCommand]
    private async Task LoadMatchesAsync()
    {
        if (IsLoading) return;

        IsLoading = true;

        try
        {
            var matches = await _matchService.GetMatchesAsync(SearchCity, DateTime.Now);
            
            Matches.Clear();
            foreach (var match in matches)
            {
                Matches.Add(match);
            }
        }
        catch (Exception ex)
        {
            await Application.Current!.MainPage!.DisplayAlert("Erreur", ex.Message, "OK");
        }
        finally
        {
            IsLoading = false;
            IsRefreshing = false;
        }
    }

    [RelayCommand]
    private async Task RefreshAsync()
    {
        IsRefreshing = true;
        await LoadMatchesAsync();
    }

    [RelayCommand]
    private async Task MatchSelectedAsync(MatchModel match)
    {
        if (match == null) return;

        await _navigationService.NavigateToAsync("matchdetail", new Dictionary<string, object>
        {
            { "MatchId", match.Id }
        });
    }

    [RelayCommand]
    private async Task SearchAsync()
    {
        await LoadMatchesAsync();
    }
}
