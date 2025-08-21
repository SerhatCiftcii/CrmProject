// CrmProject.Domain/Enums/OfferStatus.cs
using System.ComponentModel.DataAnnotations;

namespace CrmProject.Domain.Enums
{
    public enum OfferStatus
    {
        [Display(Name = "Hazırlanmadı")]
        NotPrepared,

        [Display(Name = "Hazırlandı")]
        Prepared,

        [Display(Name = "Gönderildi")]
        Sent,

        [Display(Name = "Onaylandı")]
        Approved,

        [Display(Name = "Reddedildi")]
        Rejected
    }
}