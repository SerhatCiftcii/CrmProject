// CrmProject.Domain/Entities/MailLog.cs
namespace CrmProject.Domain.Entities
{
    public class MailLog : BaseEntity
    {
        public string ToEmail { get; set; } // Kime gitti
        public string Subject { get; set; }  // Konu
        public string Body { get; set; } // İçerik
        public bool IsSuccess { get; set; } // Başarılı mı
        public string ErrorMessage { get; set; } // Hata varsa
        public DateTime SentDate { get; set; }  // Gönderim tarihi
    }
}