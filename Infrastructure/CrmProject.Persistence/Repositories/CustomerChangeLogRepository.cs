using CrmProject.Application.Interfaces;
using CrmProject.Domain.Entities;
using CrmProject.Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CrmProject.Persistence.Repositories
{
    public class CustomerChangeLogRepository : GenericRepository<CustomerChangeLog>, ICustomerChangeLogRepository
    {
        public CustomerChangeLogRepository(AppDbContext context) : base(context) { }

      
        
    }
}
