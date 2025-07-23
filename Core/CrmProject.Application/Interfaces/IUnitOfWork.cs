// CrmProject.Application/Interfaces/IUnitOfWork.cs

using CrmProject.Domain.Entities;

namespace CrmProject.Application.Interfaces
{
    public interface IUnitOfWork
    {
        // Bu property, IGenericRepository'nin bir örneğini döndürür.
        IGenericRepository<T> GetRepository<T>() where T : BaseEntity;

        // Uygulamadaki tüm değişiklikleri veritabanına kaydeder.
        Task<int> SaveChangesAsync();
    }
}