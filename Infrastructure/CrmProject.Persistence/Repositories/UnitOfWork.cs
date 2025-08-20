using CrmProject.Application.Interfaces;
using CrmProject.Domain.Entities;
using CrmProject.Infrastructure.Persistence.Context;

namespace CrmProject.Persistence.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _context;

        public UnitOfWork(AppDbContext context)
        {
            _context = context;
            Customers = new CustomerRepository(_context);
            CustomerChangeLogs = new GenericRepository<CustomerChangeLog>(_context);
        }

        public ICustomerRepository Customers { get; private set; }
        public IGenericRepository<CustomerChangeLog> CustomerChangeLogs { get; private set; }

        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }
    }
}