// CrmProject.Domain/Entities/ContactLog.cs
using CrmProject.Domain.Enums; // Enum'ları kullanmak için

namespace CrmProject.Domain.Entities
{
    public class ContactLog : BaseEntity
    {
        public int CustomerId { get; set; } // Foreign Key
        public Customer Customer { get; set; } // Navigation Property

        public string Subject { get; set; }
        public string Notes { get; set; }
        public DateTime ContactDate { get; set; }
        public ContactMethodType ContactMethod { get; set; }

        public string UserId { get; set; } // Foreign Key for AppUser (IdentityUser's Id is string)
        public AppUser User { get; set; } // Navigation Property
    }
}