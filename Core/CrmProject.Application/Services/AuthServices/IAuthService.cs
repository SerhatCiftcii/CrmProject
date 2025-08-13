using Microsoft.AspNetCore.Identity.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CrmProject.Application.Dtos.AuthDtos;

namespace CrmProject.Application.Services.AuthServices
{
    public interface IAuthService
    {
        Task<string> LoginAsync(LoginRequestDto dto);
        Task<bool> RegisterAsync(RegisterRequestDto dto);
        Task<bool> AddAdminAsync(AddAdminDto dto);
        Task<bool> SetActiveStatusAsync(AdminStatusUpdateDto dto);
        Task<List<AdminListDto>> GetAllAdminsAsync();

        Task<string> DeleteAdminAsync(string id, string currentUserId);
    }
}
