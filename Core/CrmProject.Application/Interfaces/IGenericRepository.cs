using System.Linq.Expressions;

namespace CrmProject.Application.Interfaces
{
    public interface IGenericRepository<T> where T : class
    {
        ValueTask<T?> GetByIdAsync(int id);
        IQueryable<T> GetAll();
        IQueryable<T> Where(Expression<Func<T, bool>> predicate); //şartlı sorgulama yada aktif mi sorgusu
        ValueTask AddAsync(T entity);
        void Update(T entity);
        void Delete(T entity);
    }
}
