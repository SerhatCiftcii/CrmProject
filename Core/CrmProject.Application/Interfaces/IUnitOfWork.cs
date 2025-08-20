using CrmProject.Domain.Entities;

namespace CrmProject.Application.Interfaces
{
    public interface IUnitOfWork
    {
        ICustomerRepository Customers { get; }
        IGenericRepository<CustomerChangeLog> CustomerChangeLogs { get; }
        Task<int> SaveChangesAsync();
    }
}
