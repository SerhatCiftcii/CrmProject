// CrmProject.Application/Services/ICustomerService.cs

using CrmProject.Application.DTOs.CustomerDtos; // DTO'lar için
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CrmProject.Application.Services.ServiceProducts
{
    public interface ICustomerService
    {
        // Tüm müşterileri DTO olarak getirir.
        Task<IEnumerable<CustomerDto>> GetAllCustomersAsync();

        // Belirli bir ID'ye sahip müşteriyi DTO olarak getirir.
        Task<CustomerDto> GetCustomerByIdAsync(int id);

        // Yeni bir müşteri ekler ve eklenen müşterinin DTO'sunu döndürür.
        Task<CustomerDto> AddCustomerAsync(CreateCustomerDto createCustomerDto);

        // Mevcut bir müşteriyi günceller.
        // Güncelleme metodu artık userId parametresi alıyor
        Task UpdateCustomerAsync(UpdateCustomerDto updateCustomerDto, string userId);


        // Belirli bir ID'ye sahip müşteriyi siler.
        Task DeleteCustomerAsync(int id);

        // İhtiyaç duyuldukça buraya iş mantığına özel metotlar eklenebilir.
        // Örneğin:
        // Task<IEnumerable<CustomerDto>> GetActiveCustomersAsync();
        // Task ChangeCustomerStatusAsync(int id, bool isActive);
    }
}