namespace CrmProject.Application.DTOs.CustomerChangeLogDtos
{
    public class CustomerChangeLogDto
    {
        public string CompanyName { get; set; }  // Customer.CompanyName
        public string BranchName { get; set; }   // Customer.BranchName
        public string OwnerName { get; set; }    // Customer.OwnerName
        public string FieldName { get; set; }
        public string OldValue { get; set; }
        public string NewValue { get; set; }
        public string ChangedBy { get; set; }    // AppUser.FullName veya Email
        public DateTime ChangedAt { get; set; }
    }
}
