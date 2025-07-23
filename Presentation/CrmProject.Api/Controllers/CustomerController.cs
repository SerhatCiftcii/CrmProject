// CrmProject.Api/Controllers/CustomerController.cs

using CrmProject.Application.DTOs;
using CrmProject.Application.Interfaces;
using CrmProject.Application.Services;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq; // SelectMany için
using System.Threading.Tasks;

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
                // Hata mesajlarını daha okunabilir bir formatta döndür
                var errors = ex.Errors.Select(e => new { Field = e.PropertyName, Message = e.ErrorMessage }).ToList();
                return BadRequest(new { message = "Doğrulama hataları oluştu.", errors = errors });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"Sunucu hatası oluştu: {ex.Message}" });
            }
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
                // Başarılı güncelleme için 204 No Content yanıtı döndürür (RESTful standart)
                // Eğer burada da bir mesaj isterseniz, Ok(new { message = "Müşteri başarıyla güncellendi." }) döndürebilirsiniz.
                return NoContent();
            }
            catch (KeyNotFoundException)
            {
                return NotFound(new { message = $"ID'si {id} olan müşteri bulunamadı. Güncelleme yapılamadı." });
            }
            catch (ValidationException ex)
            {
                var errors = ex.Errors.Select(e => new { Field = e.PropertyName, Message = e.ErrorMessage }).ToList();
                return BadRequest(new { message = "Doğrulama hataları oluştu.", errors = errors });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"Sunucu hatası oluştu: {ex.Message}" });
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCustomer(int id)
        {
            try
            {
                await _customerService.DeleteCustomerAsync(id);
                // Başarılı silme için 204 No Content yanıtı döndürür (RESTful standart)
                // Eğer burada da bir mesaj isterseniz, Ok(new { message = "Müşteri başarıyla silindi." }) döndürebilirsiniz.
                return NoContent();
            }
            catch (KeyNotFoundException)
            {
                return NotFound(new { message = $"ID'si {id} olan müşteri bulunamadı. Silme yapılamadı." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"Sunucu hatası oluştu: {ex.Message}" });
            }
        }
    }
}
