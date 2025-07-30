using CrmProject.Domain.Entities;

namespace CrmProject.Application.Interfaces
{
    public interface IProductRepository : IGenericRepository<Product>
    {
        Task<List<Product>> GetTopPriceProductsAsync(int count);
    }
}
