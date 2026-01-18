using GestMatch.Domain.Enums;

namespace GestMatch.MauiApp.Models;

/// <summary>
/// Modèle de match pour l'application mobile
/// </summary>
public class MatchModel
{
    public Guid Id { get; set; }
    public string TeamA { get; set; } = string.Empty;
    public string TeamB { get; set; } = string.Empty;
    public DateTime MatchDateTime { get; set; }
    public string Stadium { get; set; } = string.Empty;
    public string City { get; set; } = string.Empty;
    public string Competition { get; set; } = string.Empty;
    public MatchType MatchType { get; set; }
    public MatchStatus Status { get; set; }
    public decimal StandardTicketPrice { get; set; }
    public int TicketsRemaining { get; set; }
    public string? PosterUrl { get; set; }
    
    public string DisplayName => $"{TeamA} vs {TeamB}";
    public string DateTimeDisplay => MatchDateTime.ToString("dd MMM yyyy • HH:mm");
    public string PriceDisplay => $"{StandardTicketPrice:N0} FCFA";
}

/// <summary>
/// Modèle de billet pour l'application mobile
/// </summary>
public class TicketModel
{
    public Guid Id { get; set; }
    public string TicketNumber { get; set; } = string.Empty;
    public TicketType TicketType { get; set; }
    public TicketStatus Status { get; set; }
    public decimal Price { get; set; }
    public string QrCodeData { get; set; } = string.Empty;
    public string? QrCodeImageUrl { get; set; }
    public string MatchDescription { get; set; } = string.Empty;
    public DateTime MatchDateTime { get; set; }
    public string Stadium { get; set; } = string.Empty;
    public string City { get; set; } = string.Empty;
    
    public string StatusDisplay => Status switch
    {
        TicketStatus.Valid => "Valide",
        TicketStatus.Used => "Utilisé",
        TicketStatus.Cancelled => "Annulé",
        TicketStatus.Expired => "Expiré",
        _ => "Inconnu"
    };
    
    public Color StatusColor => Status switch
    {
        TicketStatus.Valid => Colors.Green,
        TicketStatus.Used => Colors.Gray,
        TicketStatus.Cancelled => Colors.Red,
        TicketStatus.Expired => Colors.Orange,
        _ => Colors.Black
    };
}
