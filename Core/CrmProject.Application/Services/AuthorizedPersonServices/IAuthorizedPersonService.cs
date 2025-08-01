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

        // Belirli bir ID'ye sahip yetkili kişiyi getirir. Eğer kişi bulunamazsa null döndürür.
        Task<AuthorizedPersonDto?> GetAuthorizedPersonByIdAsync(int id);

        Task AddAuthorizedPersonAsync(UpdateAuthorizedPersonDto updateAuthorizedPersonDto );

        // Belirli bir ID'ye sahip yetkili kişiyi siler.
        Task DeleteAuthorizedPersonAsync(int id);
    }
}
