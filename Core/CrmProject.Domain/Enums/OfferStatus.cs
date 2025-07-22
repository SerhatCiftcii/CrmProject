// CrmProject.Domain/Enums/OfferStatus.cs
namespace CrmProject.Domain.Enums
{
    public enum OfferStatus
    {
        NotPrepared, // Hazırlanmadı
        Prepared,    // Hazırlandı
        Sent,        // Gönderildi
        Approved,    // Onaylandı
        Rejected     // Reddedildi
    }
}