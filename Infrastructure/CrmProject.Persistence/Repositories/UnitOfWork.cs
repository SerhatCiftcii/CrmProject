// CrmProject.Persistence/Repositories/UnitOfWork.cs

using CrmProject.Application.Interfaces;
using CrmProject.Domain.Entities;
using CrmProject.Infrastructure.Persistence.Context;

namespace CrmProject.Persistence.Repositories // Namespace güncellendi
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _context;

        public UnitOfWork(AppDbContext context)
        {
            _context = context;
        }

        public IGenericRepository<T> GetRepository<T>() where T : BaseEntity
        {
            return new GenericRepository<T>(_context);
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }
    }
}