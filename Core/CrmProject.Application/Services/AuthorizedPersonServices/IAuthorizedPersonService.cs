using CrmProject.Application.Dtos.AuthorizedPersonDtos;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrmProject.Application.Services.AuthorizedPersonServices
{
    public interface IAuthorizedPersonService 
    {
        Task<List<AuthorizedPersonDto>> GetAllAuthorizedPersonAsync();
        Task<AuthorizedPersonDto?> GetAuthorizedPersonByIdAsync(int id);
        Task<AuthorizedPersonDto> AddAuthorizedPersonAsync(CreateAuthorizedPersonDto dto);
        Task UpdateAuthorizedPersonAsync(UpdateAuthorizedPersonDto dto);
        Task DeleteAuthorizedPersonAsync(int id);
    }
}
    