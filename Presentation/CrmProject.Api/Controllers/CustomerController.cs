// CrmProject.Api/Controllers/CustomerController.cs

using CrmProject.Application.DTOs.CustomerDtos;
using CrmProject.Application.Interfaces;
using CrmProject.Application.Services.ServiceProducts;

// using CrmProject.Application.Services.ServiceProducts; // Bu using'e burada gerek yok
using FluentValidation; // ValidationException için
using Microsoft.AspNetCore.Mvc;
using System; // KeyNotFoundException için
using System.Collections.Generic;
using System.Linq;

namespace CrmProject.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        private readonly ICustomerService _customerService;

        public CustomerController(ICustomerService customerService)
        {
            _customerService = customerService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllCustomers()
        {
            var customers = await _customerService.GetAllCustomersAsync();
            return Ok(customers);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetCustomerById(int id)
        {
            var customer = await _customerService.GetCustomerByIdAsync(id);
            if (customer == null)
            {
                // Müşteri bulunamazsa 404 Not Found yanıtı ile açıklayıcı bir mesaj döndürür
                return NotFound(new { message = $"ID'si {id} olan müşteri bulunamadı." });
            }
            return Ok(customer);
        }

        [HttpPost]
        public async Task<IActionResult> AddCustomer([FromBody] CreateCustomerDto createCustomerDto)
        {
            try
            {
                var addedCustomer = await _customerService.AddCustomerAsync(createCustomerDto);
                // 201 Created yanıtı ile oluşturulan müşteriyi ve bir başarı mesajını döndürür
                return CreatedAtAction(nameof(GetCustomerById), new { id = addedCustomer.Id }, new { message = "Müşteri başarıyla eklendi.", customer = addedCustomer });
            }
            catch (ValidationException ex)
            {
                // FluentValidation hatalarını yakala ve 400 Bad Request döndür
                var errors = ex.Errors.Select(e => new { Field = e.PropertyName, Message = e.ErrorMessage }).ToList();
                return BadRequest(new { message = "Doğrulama hataları oluştu.", errors = errors });
            }
            // Genel 'catch (Exception ex)' bloğu kaldırıldı. Beklenmeyen diğer hatalar için varsayılan 500 dönecektir.
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCustomer(int id, [FromBody] UpdateCustomerDto updateCustomerDto)
        {
            if (id != updateCustomerDto.Id)
            {
                return BadRequest(new { message = "URL'deki ID ile istek gövdesindeki ID uyuşmuyor." });
            }

            try
            {
                await _customerService.UpdateCustomerAsync(updateCustomerDto);
                // Başarılı güncelleme için 200 OK yanıtı ile mesaj döndürülür.
                return Ok(new { message = $"ID'si {id} olan müşteri başarıyla güncellendi." });
            }
            catch (KeyNotFoundException) // KeyNotFoundException'ı yakalamaya devam ediyoruz
            {
                return NotFound(new { message = $"ID'si {id} olan müşteri bulunamadı. Güncelleme yapılamadı." });
            }
            catch (ValidationException ex) // ValidationException'ı yakalamaya devam ediyoruz
            {
                var errors = ex.Errors.Select(e => new { Field = e.PropertyName, Message = e.ErrorMessage }).ToList();
                return BadRequest(new { message = "Doğrulama hataları oluştu.", errors = errors });
            }
            // Genel 'catch (Exception ex)' bloğu kaldırıldı. Beklenmeyen diğer hatalar için varsayılan 500 dönecektir.
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCustomer(int id)
        {
            // Önce müşterinin varlığını kontrol et
            var existingCustomer = await _customerService.GetCustomerByIdAsync(id);
            if (existingCustomer == null)
            {
                // Müşteri bulunamazsa 404 Not Found döndürülür.
                return NotFound(new { message = $"ID'si {id} olan müşteri bulunamadı. Silme yapılamadı." });
            }

            // Müşteri varsa silme işlemini çağır (Servis artık exception fırlatmayacak)
            await _customerService.DeleteCustomerAsync(id);
            // Başarılı silme için 200 OK yanıtı ile mesaj döndürülür.
            return Ok(new { message = $"ID'si {id} olan müşteri başarıyla silindi." });
        }
    }
}
