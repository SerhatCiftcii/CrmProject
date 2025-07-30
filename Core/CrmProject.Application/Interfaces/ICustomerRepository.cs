using CrmProject.Domain.Entities;

namespace CrmProject.Application.Interfaces
{
    public interface ICustomerRepository : IGenericRepository<Customer>
    {
        Task<Customer?> GetCustomerWithProductsAsync(int id);
        Task<List<Customer>> GetAllWithProductsAsync();
    }
}
