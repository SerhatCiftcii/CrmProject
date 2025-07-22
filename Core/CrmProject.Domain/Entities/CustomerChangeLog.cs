// CrmProject.Domain/Entities/CustomerChangeLog.cs
namespace CrmProject.Domain.Entities
{
    public class CustomerChangeLog : BaseEntity
    {
        public int CustomerId { get; set; } // Foreign Key
        public Customer Customer { get; set; } // Navigation Property

        public string FieldName { get; set; }
        public string OldValue { get; set; }
        public string NewValue { get; set; }

        public string ChangedByUserId { get; set; } // Foreign Key
        public AppUser ChangedByUser { get; set; } // Navigation Property

        public DateTime ChangedAt { get; set; }
    }
}