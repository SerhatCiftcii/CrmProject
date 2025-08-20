namespace CrmProject.Application.DTOs.CustomerChangeLogDtos
{
    public class CreateCustomerChangeLogDto
    {
        public int CustomerId { get; set; }
        public string FieldName { get; set; }
        public string OldValue { get; set; }
        public string NewValue { get; set; }
        public string ChangedByUserId { get; set; }
    }
}
