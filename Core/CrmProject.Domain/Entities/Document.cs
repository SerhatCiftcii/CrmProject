// CrmProject.Domain/Entities/Document.cs
namespace CrmProject.Domain.Entities
{
    public class Document : BaseEntity
    {
        public int CustomerId { get; set; } // Foreign Key
        public Customer Customer { get; set; } // Navigation Property

        public string FileName { get; set; }
        public string FileType { get; set; }
        public byte[] FileData { get; set; }
        public DateTime UploadedAt { get; set; }
    }
}