using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrmProject.Domain.Entities
{
    public class Product : BaseEntity
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }

        // Navigation Properties for Many-to-Many
        public ICollection<CustomerProduct> CustomerProducts { get; set; }
        public ICollection<MaintenanceProduct> MaintenanceProducts { get; set; }
    }
}
