using CrmProject.Domain.Enums;
using System;
using System.Collections.Generic;

namespace CrmProject.Application.Dtos.MaintenanceDtos
{
    public class CreateMaintenanceDto
    {
        public int CustomerId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public DateTime? PassportCreatedDate { get; set; }
        public OfferStatus OfferStatus { get; set; }
        public ContractStatus ContractStatus { get; set; }
        public LicenseStatus LicenseStatus { get; set; }
        public FirmSituation FirmSituation { get; set; }

        
        public string? Description { get; set; }
        public bool ExtendBy6Months { get; set; }
        public bool ExtendBy1Year { get; set; }

        // Varsayılan olarak boş bir liste atanarak null referans hataları engellendi.
        public List<int> ProductIds { get; set; } = new List<int>();
    }
}