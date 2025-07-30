using CrmProject.Application.Interfaces;
using CrmProject.Infrastructure.Persistence.Context;

namespace CrmProject.Persistence.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _context;

        public UnitOfWork(AppDbContext context)
        {
            _context = context;
        }

        public async Task<int> SaveChangesAsync() => await _context.SaveChangesAsync();
    }
}
