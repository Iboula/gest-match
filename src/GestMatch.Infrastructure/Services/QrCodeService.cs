using GestMatch.Application.Interfaces;
using QRCoder;
using System.Text;

namespace GestMatch.Infrastructure.Services;

/// <summary>
/// Implémentation du service de génération de QR Code
/// </summary>
public class QrCodeService : IQrCodeService
{
    private const string SecretKey = "GestMatch-Secret-2026"; // En production, utiliser un secret sécurisé

    public string GenerateQrCodeData(Guid ticketId, string ticketNumber)
    {
        // Format: TicketId|TicketNumber|Timestamp|Signature
        var timestamp = DateTime.UtcNow.Ticks;
        var data = $"{ticketId}|{ticketNumber}|{timestamp}";
        var signature = GenerateSignature(data);
        
        return $"{data}|{signature}";
    }

    public string GenerateQrCodeImage(string qrCodeData)
    {
        using var qrGenerator = new QRCodeGenerator();
        using var qrCodeInfo = qrGenerator.CreateQrCode(qrCodeData, QRCodeGenerator.ECCLevel.Q);
        using var qrCode = new PngByteQRCode(qrCodeInfo);
        
        var qrCodeBytes = qrCode.GetGraphic(20);
        return Convert.ToBase64String(qrCodeBytes);
    }

    public bool ValidateQrCodeData(string qrCodeData, out Guid ticketId)
    {
        ticketId = Guid.Empty;

        try
        {
            var parts = qrCodeData.Split('|');
            if (parts.Length != 4)
                return false;

            if (!Guid.TryParse(parts[0], out ticketId))
                return false;

            var data = $"{parts[0]}|{parts[1]}|{parts[2]}";
            var providedSignature = parts[3];
            var expectedSignature = GenerateSignature(data);

            return providedSignature == expectedSignature;
        }
        catch
        {
            return false;
        }
    }

    private static string GenerateSignature(string data)
    {
        using var sha256 = System.Security.Cryptography.SHA256.Create();
        var input = Encoding.UTF8.GetBytes(data + SecretKey);
        var hash = sha256.ComputeHash(input);
        return Convert.ToBase64String(hash);
    }
}
