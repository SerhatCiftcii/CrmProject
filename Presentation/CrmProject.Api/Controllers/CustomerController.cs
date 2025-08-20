using CrmProject.Application.DTOs.CustomerDtos;
using CrmProject.Application.Interfaces;
using CrmProject.Application.Services.ServiceProducts;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
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
                return NotFound(new { message = $"ID'si {id} olan müşteri bulunamadı." });

            return Ok(customer);
        }

        [HttpPost]
        public async Task<IActionResult> AddCustomer([FromBody] CreateCustomerDto dto)
        {
            try
            {
                var added = await _customerService.AddCustomerAsync(dto);
                return CreatedAtAction(nameof(GetCustomerById), new { id = added.Id }, added);
            }
            catch (ValidationException ex)
            {
                var errors = ex.Errors?.Select(e => new { Field = e.PropertyName, Message = e.ErrorMessage }) ?? Enumerable.Empty<object>();

                return BadRequest(new
                {
                    message = string.IsNullOrEmpty(ex.Message) ? "Doğrulama hataları oluştu" : ex.Message,
                    errors
                });
            }
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCustomer(int id, [FromBody] UpdateCustomerDto dto)
        {
            if (id != dto.Id)
                return BadRequest(new { message = "Id eşleşmiyor." });

            // JWT'den userId çek
            var userId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
                return Unauthorized(new { message = "Kullanıcı bilgisi bulunamadı." });

            try
            {
                await _customerService.UpdateCustomerAsync(dto, userId);
                return Ok(new { message = "Müşteri güncellendi." });
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCustomer(int id)
        {
            var existing = await _customerService.GetCustomerByIdAsync(id);
            if (existing == null)
                return NotFound(new { message = "Müşteri bulunamadı" });

            await _customerService.DeleteCustomerAsync(id);
            return Ok(new { message = "Müşteri silindi" });
        }
    }
}
