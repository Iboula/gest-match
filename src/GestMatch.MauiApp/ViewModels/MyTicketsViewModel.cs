using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using GestMatch.MauiApp.Models;
using GestMatch.MauiApp.Services;
using System.Collections.ObjectModel;

namespace GestMatch.MauiApp.ViewModels;

/// <summary>
/// ViewModel pour mes billets
/// </summary>
public partial class MyTicketsViewModel : ObservableObject
{
    private readonly ITicketService _ticketService;
    private readonly INavigationService _navigationService;

    [ObservableProperty]
    private ObservableCollection<TicketModel> _tickets = new();

    [ObservableProperty]
    private bool _isLoading;

    [ObservableProperty]
    private bool _isRefreshing;

    public MyTicketsViewModel(ITicketService ticketService, INavigationService navigationService)
    {
        _ticketService = ticketService;
        _navigationService = navigationService;
    }

    public async Task InitializeAsync()
    {
        await LoadTicketsAsync();
    }

    [RelayCommand]
    private async Task LoadTicketsAsync()
    {
        if (IsLoading) return;

        IsLoading = true;

        try
        {
            var tickets = await _ticketService.GetMyTicketsAsync();
            
            Tickets.Clear();
            foreach (var ticket in tickets)
            {
                Tickets.Add(ticket);
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
        await LoadTicketsAsync();
    }

    [RelayCommand]
    private async Task TicketSelectedAsync(TicketModel ticket)
    {
        if (ticket == null) return;

        await _navigationService.NavigateToAsync("ticketdetail", new Dictionary<string, object>
        {
            { "TicketId", ticket.Id }
        });
    }
}
