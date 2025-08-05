using CrmProject.Domain.Entities;
using CrmProject.Infrastructure.Persistence.Context;
using CrmProject.Persistence.Repositories.IMaintenanceRepositories;
using Microsoft.EntityFrameworkCore;

namespace CrmProject.Persistence.Repositories.MaintenanceRepositories
{
    public class MaintenanceRepository : GenericRepository<Maintenance>, IMaintenanceRepository
    {
        public MaintenanceRepository(AppDbContext context) : base(context) { }

        
        /// Belirli bir bakım kaydını, müşteri ve ürün bilgileriyle birlikte getirir.
    
        public async Task<Maintenance?> GetMaintenanceWithDetailsAsync(int id)
        {
            return await Context.Maintenances
     .AsNoTracking()
     .Include(m => m.Customer)
     .Include(m => m.MaintenanceProducts)
         .ThenInclude(mp => mp.Product)
     .FirstOrDefaultAsync(m => m.Id == id);
        }

       
        /// Tüm bakım kayıtlarını, müşteri ve ürün bilgileriyle birlikte getirir.
    
        public async Task<List<Maintenance>> GetAllMaintenanceWithDetailsAsync()
        {
            return await Context.Maintenances
                .Include(m => m.Customer)
                .Include(m => m.MaintenanceProducts)
                    .ThenInclude(mp => mp.Product)
                .ToListAsync();
        }
    }
}
