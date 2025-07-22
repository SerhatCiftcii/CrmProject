// CrmProject.Domain/Entities/MaintenanceProduct.cs
namespace CrmProject.Domain.Entities
{
    public class MaintenanceProduct
    {
        public int MaintenanceId { get; set; } // Foreign Key part of Composite Key
        public Maintenance Maintenance { get; set; } // Navigation Property

        public int ProductId { get; set; } // Foreign Key part of Composite Key
        public Product Product { get; set; } // Navigation Property
    }
}