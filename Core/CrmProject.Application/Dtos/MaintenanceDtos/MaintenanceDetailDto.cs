
using CrmProject.Application.DTOs.ProductDtos;
using CrmProject.Domain.Enums;
using System;
using System.Collections.Generic;

namespace CrmProject.Application.Dtos.MaintenanceDtos
{
    public class MaintenanceDetailDto
    {
        public int Id { get; set; }
        public int CustomerId { get; set; }
        public string CompanyName { get; set; } // İlişkili müşteri verisini göstermek için
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public DateTime? PassportCreatedDate { get; set; }

        // Enum değerlerini string olarak göstermek daha esnek olabilir.
        public string OfferStatus { get; set; }
        public string ContractStatus { get; set; }
        public string LicenseStatus { get; set; }
        public string FirmSituation { get; set; }
        public string? Description { get; set; } // Alan nullable olarak güncellendi
        public bool ExtendBy6Months { get; set; }
        public bool ExtendBy1Year { get; set; }

        // Varsayılan olarak boş bir liste atanarak null referans hataları için.
        public List<ProductDto> Products { get; set; } = new List<ProductDto>();
    }
}