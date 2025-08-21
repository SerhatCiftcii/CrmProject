// CrmProject.Domain/Enums/ContractStatus.cs
using System.ComponentModel.DataAnnotations;

namespace CrmProject.Domain.Enums
{
    public enum ContractStatus
    {
        [Display(Name = "Gönderilmedi")]
        NotSent,

        [Display(Name = "Gönderildi")]
        Sent,

        [Display(Name = "İmzalandı")]
        Signed,

        [Display(Name = "İptal Edildi")]
        Cancelled
    }
}