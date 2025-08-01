// CrmProject.Application/DTOs/AuthorizedPersonDtos/UpdateAuthorizedPersonDto.cs

using System; // DateTime için

namespace CrmProject.Application.Dtos.AuthorizedPersonDtos
{
    public class UpdateAuthorizedPersonDto
    {
        public int Id { get; set; } // Güncellenecek yetkili kişinin ID'si
        public int CustomerId { get; set; } // Hangi müşteriye ait olduğu (güncelleme sırasında da gönderilebilir)
        public string FullName { get; set; }
        public string Title { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public DateTime? BirthDate { get; set; }
        public bool IsActive { get; set; }
        public string Notes { get; set; }
    }
}
