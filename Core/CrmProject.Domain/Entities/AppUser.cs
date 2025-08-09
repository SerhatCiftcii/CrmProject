// CrmProject.Domain/Entities/AppUser.cs
using Microsoft.AspNetCore.Identity;

namespace CrmProject.Domain.Entities
{
    public class AppUser : IdentityUser
    {
        public string FullName { get; set; }
        public bool IsActive { get; set; } = true;
        public bool IsSuperAdmin { get; set; } = false;

        // Navigation Properties (Bu AppUser'ın oluşturduğu loglar)
        public ICollection<ChangeLog> ChangeLogs { get; set; }
        public ICollection<ContactLog> ContactLogs { get; set; } 
        public ICollection<CustomerChangeLog> CustomerChangeLogs { get; set; }

        public ICollection<AuthorizedPerson> AuthorizedPersons { get; set; }
    }
}