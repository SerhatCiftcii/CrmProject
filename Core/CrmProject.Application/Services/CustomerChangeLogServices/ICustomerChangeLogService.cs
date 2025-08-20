using CrmProject.Application.DTOs.CustomerChangeLogDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrmProject.Application.Services.CustomerChangeLogServices
{
    public interface ICustomerChangeLogService
    {
        // Belirli bir müşterinin tüm değişiklik loglarını getirir
        Task<IEnumerable<CustomerChangeLogDto>> GetLogsByCustomerIdAsync(int customerId);

        // Yeni bir değişiklik logu ekler
        Task AddChangeLogAsync(CreateCustomerChangeLogDto dto);

        Task<IEnumerable<CustomerChangeLogDto>> GetAllLogsAsync();
    }
}
