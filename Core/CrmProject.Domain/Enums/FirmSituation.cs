// CrmProject.Domain/Enums/FirmSituation.cs
using System.ComponentModel.DataAnnotations;

namespace CrmProject.Domain.Enums
{
    public enum FirmSituation
    {
        [Display(Name = "Devam Ediyor")]
        Continues,

        [Display(Name = "Durduruldu")]
        Paused,

        [Display(Name = "Tamamlandı")]
        Finished,

        [Display(Name = "İptal Edildi")]
        Cancelled
    }
}