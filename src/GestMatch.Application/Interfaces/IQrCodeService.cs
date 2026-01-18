namespace GestMatch.Application.Interfaces;

/// <summary>
/// Service de génération de QR Code
/// </summary>
public interface IQrCodeService
{
    /// <summary>
    /// Générer les données d'un QR Code pour un billet
    /// </summary>
    string GenerateQrCodeData(Guid ticketId, string ticketNumber);
    
    /// <summary>
    /// Générer une image QR Code en base64
    /// </summary>
    string GenerateQrCodeImage(string qrCodeData);
    
    /// <summary>
    /// Vérifier la validité des données d'un QR Code
    /// </summary>
    bool ValidateQrCodeData(string qrCodeData, out Guid ticketId);
}
