// CrmProject.Domain/Entities/ChangeLog.cs
namespace CrmProject.Domain.Entities
{
    public class ChangeLog : BaseEntity
    {
        public string TableName { get; set; }
        public string FieldName { get; set; }
        public string OldValue { get; set; }
        public string NewValue { get; set; }
        public DateTime ChangeDate { get; set; }
        public string RecordId { get; set; }

        public string UserId { get; set; } // Foreign Key for AppUser
        public AppUser User { get; set; } // Navigation Property
    }
}