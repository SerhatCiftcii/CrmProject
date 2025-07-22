// CrmProject.Domain/Entities/MailLog.cs
namespace CrmProject.Domain.Entities
{
    public class MailLog : BaseEntity
    {
        public string ToEmail { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        public bool IsSuccess { get; set; }
        public string ErrorMessage { get; set; }
        public DateTime SentDate { get; set; }
    }
}