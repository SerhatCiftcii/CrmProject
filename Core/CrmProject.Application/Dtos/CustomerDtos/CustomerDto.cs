﻿
using CrmProject.Application.DTOs.ProductDtos;
using CrmProject.Domain.Enums;

namespace CrmProject.Application.DTOs.CustomerDtos
{
    public class CustomerDto
    {
        public int Id { get; set; }
        public string CompanyName { get; set; }
        public string BranchName { get; set; }
        public string OwnerName { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string City { get; set; }
        public string District { get; set; }
        public string Address { get; set; }
        public string TaxNumber { get; set; }
        public string TaxOffice { get; set; }
        public string WebSite { get; set; }
        public DateTime? SalesDate { get; set; }
        public CustomerStatus Status { get; set; } // bool IsActive yerine CustomerStatus Status
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        // Müşterinin sahip olduğu ürünlerin listesi
        public List<ProductDto> Products { get; set; }
    }
}
