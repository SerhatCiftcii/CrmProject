using CrmProject.Application.Interfaces;
using CrmProject.Domain.Entities;
using CrmProject.Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace CrmProject.Persistence.Repositories
{
    public class CustomerRepository : GenericRepository<Customer>, ICustomerRepository
    {
        public CustomerRepository(AppDbContext context) : base(context) { }

        public async Task<Customer?> GetCustomerWithProductsAsync(int id)
        {
            return await Context.Customers
                .Include(c => c.CustomerProducts)
                .ThenInclude(cp => cp.Product)
                .FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<List<Customer>> GetAllWithProductsAsync()
        {
            return await Context.Customers
                .Include(c => c.CustomerProducts)
                .ThenInclude(cp => cp.Product)
                .ToListAsync();
        }
    }
}
