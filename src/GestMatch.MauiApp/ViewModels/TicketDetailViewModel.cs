using CommunityToolkit.Mvvm.ComponentModel;
using GestMatch.MauiApp.Models;
using GestMatch.MauiApp.Services;

namespace GestMatch.MauiApp.ViewModels;

/// <summary>
/// ViewModel pour les détails d'un billet
/// </summary>
[QueryProperty(nameof(TicketId), "TicketId")]
public partial class TicketDetailViewModel : ObservableObject
{
    private readonly ITicketService _ticketService;

    [ObservableProperty]
    private Guid _ticketId;

    [ObservableProperty]
    private TicketModel? _ticket;

    [ObservableProperty]
    private bool _isLoading;

    public TicketDetailViewModel(ITicketService ticketService)
    {
        _ticketService = ticketService;
    }

    partial void OnTicketIdChanged(Guid value)
    {
        _ = LoadTicketAsync();
    }

    private async Task LoadTicketAsync()
    {
        if (TicketId == Guid.Empty) return;

        IsLoading = true;

        try
        {
            Ticket = await _ticketService.GetTicketByIdAsync(TicketId);
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
}
