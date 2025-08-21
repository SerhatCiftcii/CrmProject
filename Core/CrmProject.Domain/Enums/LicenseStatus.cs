// CrmProject.Domain/Enums/LicenseStatus.cs
using System.ComponentModel.DataAnnotations;

namespace CrmProject.Domain.Enums
{
    public enum LicenseStatus
    {
        [Display(Name = "Aktif")]
        Active,

        [Display(Name = "Pasif")]
        Passive,

        [Display(Name = "Bekliyor")]
        Awaiting,

        [Display(Name = "Süresi Doldu")]
        Expired
    }
}