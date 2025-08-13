// CrmProject.Domain/Entities/AuthorizedPerson.cs
namespace CrmProject.Domain.Entities
{
    public class AuthorizedPerson : BaseEntity
    {
        public int CustomerId { get; set; } // Foreign Key
        public Customer Customer { get; set; } // Navigation Property

        public string FullName { get; set; }
        public string Title { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public DateTime? BirthDate { get; set; }
        public string Notes { get; set; }

        public bool IsActive { get; set; } = true; // Aktif/pasif durumu
    }
}