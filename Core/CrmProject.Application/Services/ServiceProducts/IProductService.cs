// CrmProject.Application/Services/IProductService.cs

using CrmProject.Application.DTOs.ProductDtos; // Product DTO'ları için
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CrmProject.Application.Services.ServiceProducts
{
    public interface IProductService
    {
        // Tüm ürünleri DTO olarak getirir.
        Task<IEnumerable<ProductDto>> GetAllProductsAsync();

        // Belirli bir ID'ye sahip ürünü DTO olarak getirir.
        Task<ProductDto> GetProductByIdAsync(int id);

        // Yeni bir ürün ekler ve eklenen ürünün DTO'sunu döndürür.
        Task<ProductDto> AddProductAsync(CreateProductDto createProductDto);

        // Mevcut bir ürünü günceller.
        Task UpdateProductAsync(UpdateProductDto updateProductDto);

        // Belirli bir ID'ye sahip ürünü siler.
        Task DeleteProductAsync(int id);

        // İhtiyaç duyuldukça buraya iş mantığına özel metotlar eklenebilir.
    }
}
