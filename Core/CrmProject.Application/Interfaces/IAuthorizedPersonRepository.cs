using CrmProject.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrmProject.Application.Interfaces
{
    // IAuthorizedPersonRepository arayüzü, AuthorizedPerson entity'sine özel veri
    // erişim işlemlerini tanımlar. IGenericRepository'den miras alır.
    public interface IAuthorizedPersonRepository :IGenericRepository<AuthorizedPerson>  
    {
        //ilerde özel metot olursa buraya yazcam.
    }
}
