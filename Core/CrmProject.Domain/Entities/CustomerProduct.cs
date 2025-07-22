// CrmProject.Domain/Entities/CustomerProduct.cs
namespace CrmProject.Domain.Entities
{
    public class CustomerProduct
    {
        public int CustomerId { get; set; } // Foreign Key part of Composite Key
        public Customer Customer { get; set; } // Navigation Property

        public int ProductId { get; set; } // Foreign Key part of Composite Key
        public Product Product { get; set; } // Navigation Property

        public DateTime AssignedDate { get; set; }
    }
}