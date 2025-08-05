using CrmProject.Application.Interfaces;
using CrmProject.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CrmProject.Persistence.Repositories.IMaintenanceRepositories
{
    // Maintenance entity'sine özel metotların sözleşmesini tanımlar.
    public interface IMaintenanceRepository : IGenericRepository<Maintenance>
    {
       
        /// Belirli bir Id'ye sahip bakım kaydını, ilişkili müşteri ve ürünlerle birlikte getirir.
      
        Task<Maintenance?> GetMaintenanceWithDetailsAsync(int id);

     
        /// Tüm bakım kayıtlarını, ilişkili müşteri ve ürünlerle birlikte getirir.
        
        Task<List<Maintenance>> GetAllMaintenanceWithDetailsAsync();
    }
}
