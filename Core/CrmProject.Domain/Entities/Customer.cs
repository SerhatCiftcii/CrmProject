// CrmProject.Domain/Entities/Customer.cs
using CrmProject.Domain.Enums; // Enum'ları kullanmak için

namespace CrmProject.Domain.Entities
{
    public class Customer : BaseEntity
    {
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
        public CustomerStatus Status { get; set; }

        // Navigation Properties
        public ICollection<AuthorizedPerson> AuthorizedPeople { get; set; }
        public ICollection<Maintenance> Maintenances { get; set; }
        public ICollection<CustomerProduct> CustomerProducts { get; set; }
        public ICollection<ContactLog> ContactLogs { get; set; }
        public ICollection<Document> Documents { get; set; }
        public ICollection<CustomerChangeLog> CustomerChangeLogs { get; set; } // YENİ EKLENEN
    }
}