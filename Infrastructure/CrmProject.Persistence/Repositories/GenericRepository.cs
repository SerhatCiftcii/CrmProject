using CrmProject.Application.Interfaces;
using CrmProject.Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace CrmProject.Persistence.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        protected readonly AppDbContext Context;
        protected readonly DbSet<T> DbSet;

        public GenericRepository(AppDbContext context)
        {
            Context = context;
            DbSet = context.Set<T>();
        }

        public IQueryable<T> GetAll() => DbSet.AsNoTracking();

        public IQueryable<T> Where(Expression<Func<T, bool>> predicate)
            => DbSet.Where(predicate).AsNoTracking();

        public ValueTask<T?> GetByIdAsync(int id) => DbSet.FindAsync(id);

        public async ValueTask AddAsync(T entity) => await DbSet.AddAsync(entity);

        public void Update(T entity) => DbSet.Update(entity);

        public void Delete(T entity) => DbSet.Remove(entity);
    }
}
