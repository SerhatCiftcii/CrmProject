// CrmProject.Application/Interfaces/IGenericRepository.cs

using CrmProject.Domain.Entities;
using System.Linq.Expressions;

namespace CrmProject.Application.Interfaces
{
    public interface IGenericRepository<T> where T : BaseEntity
    {
        // Tüm entity'leri getirir.
        Task<IReadOnlyList<T>> GetAllAsync();

        // Belirli bir ID'ye sahip entity'yi getirir.
        Task<T> GetByIdAsync(int id);

        // Belirli bir koşula uyan entity'leri getirir.
        Task<IReadOnlyList<T>> GetWhereAsync(Expression<Func<T, bool>> expression);

        // Yeni bir entity ekler.
        Task AddAsync(T entity);

        // Birden fazla yeni entity ekler.
        Task AddRangeAsync(IEnumerable<T> entities);

        // Bir entity'yi günceller.
        void Update(T entity);

        // Bir entity'yi siler.
        void Remove(T entity);

        // Birden fazla entity'yi siler.
        void RemoveRange(IEnumerable<T> entities);
        // İlişkili navigation property'leri Include ederek tüm entity'leri getirir.
        Task<IReadOnlyList<T>> GetAllWithIncludesAsync(params Expression<Func<T, object>>[] includes);

        // İlişkili navigation property'leri Include ederek ID'ye göre entity getirir.
        Task<T> GetByIdWithIncludesAsync(int id, params Expression<Func<T, object>>[] includes);
        IQueryable<T> Query();

    }
}