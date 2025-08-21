using CrmProject.Application.Dtos.MaintenanceDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrmProject.Application.Services.MaintenanceServices
{
    public interface IMaintenanceService 
    {
        Task<List<MaintenanceDetailDto>> GetAllMaintenanceWithDetailsAsync();
        Task<MaintenanceDetailDto?> GetByIdAsync(int id);
        Task<MaintenanceDetailDto> AddMaintanenceAsync(CreateMaintenanceDto createMaintenanceDto);

        Task UpdateMaintenanceDto(UpdateMaintenanceDto dto, string updatedByUserName);
        Task DeleteMaintenanceAsync(int id);
    }
}
