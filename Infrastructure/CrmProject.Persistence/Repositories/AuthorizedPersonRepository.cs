using CrmProject.Application.Interfaces;
using CrmProject.Domain.Entities;
using CrmProject.Infrastructure.Persistence.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrmProject.Persistence.Repositories
{
    public class AuthorizedPersonRepository :GenericRepository<AuthorizedPerson>, IAuthorizedPersonRepository
    {
        public AuthorizedPersonRepository(AppDbContext context) : base(context) { }
        // Burada AuthorizedPerson'a özel metotlar
        // Örneğin, yetkili kişilerin belirli bir kritere göre filtrelenmesi gibi.
    }
}
