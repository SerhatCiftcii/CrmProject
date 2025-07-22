// CrmProject.Domain/Entities/AppUser.cs
using Microsoft.AspNetCore.Identity; // IdentityUser için

namespace CrmProject.Domain.Entities
{
    public class AppUser : IdentityUser // IdentityUser'dan miras alıyoruz
    {
        public string FullName { get; set; } // Ad Soyad
        public bool IsActive { get; set; } = true; // Aktif mi (Admin devre dışı bırakabilir)
        public bool IsSuperAdmin { get; set; } = false; // Ana yönetici mi

        // Navigation Properties
        public ICollection<ChangeLog> ChangeLogs { get; set; } // Bu kullanıcının yaptığı genel değişiklik logları
    }
}