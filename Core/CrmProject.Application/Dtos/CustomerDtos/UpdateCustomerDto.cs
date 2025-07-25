

namespace CrmProject.Application.DTOs.CustomerDtos
{
    public class UpdateCustomerDto
    {
        public int Id { get; set; } // Güncellenecek müşterinin ID'si
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
        public bool IsActive { get; set; } // Güncelleme sırasında aktiflik durumu da değişebilir
    }
}
