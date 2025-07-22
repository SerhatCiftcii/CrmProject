// CrmProject.Domain/Entities/Maintenance.cs
using CrmProject.Domain.Enums; // Enum'ları kullanmak için

namespace CrmProject.Domain.Entities
{
    public class Maintenance : BaseEntity
    {
        public int CustomerId { get; set; } // Foreign Key
        public Customer Customer { get; set; } // Navigation Property

        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public DateTime? PassportCreatedDate { get; set; }

        public OfferStatus OfferStatus { get; set; }
        public ContractStatus ContractStatus { get; set; }
        public LicenseStatus LicenseStatus { get; set; }
        public FirmSituation FirmSituation { get; set; }

        public string Description { get; set; }
        public bool ExtendBy6Months { get; set; }
        public bool ExtendBy1Year { get; set; }

        // Navigation Property for Many-to-Many
        public ICollection<MaintenanceProduct> MaintenanceProducts { get; set; }
    }
}