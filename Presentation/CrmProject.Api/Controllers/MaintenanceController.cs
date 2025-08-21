using CrmProject.Application.Dtos.MaintenanceDtos;
using CrmProject.Application.Services.MaintenanceServices;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CrmProject.Api.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class MaintenanceController : ControllerBase
    {
        private readonly IMaintenanceService _maintenanceService;

        public MaintenanceController(IMaintenanceService maintenanceService)
        {
            _maintenanceService = maintenanceService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var list = await _maintenanceService.GetAllMaintenanceWithDetailsAsync();
            return Ok(list);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var maintenance = await _maintenanceService.GetByIdAsync(id);
            if (maintenance == null)
                return NotFound(new { message = $"ID'si {id} olan bakım kaydı bulunamadı." });

            return Ok(maintenance);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateMaintenanceDto dto)
        {
            try
            {
                var created = await _maintenanceService.AddMaintanenceAsync(dto);
                return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
            }
            catch (ValidationException ex)
            {
                var errors = ex.Errors != null
                    ? ex.Errors.Select(e => new { Field = e.PropertyName, Message = e.ErrorMessage })
                    : null;

                return BadRequest(new { message = ex.Message, errors });
            }
        }


        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateMaintenanceDto dto)
        {
            if (id != dto.Id)
                return BadRequest(new { message = "URL'deki ID ile DTO'daki ID uyuşmuyor." });

            try
            {
                // JWT ile login olmuş kullanıcının adını alıyoruz
                var updatedByUserName = User?.Identity?.Name ?? "Bilinmeyen Kullanıcı";

                await _maintenanceService.UpdateMaintenanceDto(dto, updatedByUserName);
                return Ok(new { message = "Bakım kaydı başarıyla güncellendi." });
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (ValidationException ex)
            {
                var errors = ex.Errors?.Select(e => new { e.PropertyName, e.ErrorMessage });
                return BadRequest(new { message = "Doğrulama hataları oluştu.", errors });
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _maintenanceService.DeleteMaintenanceAsync(id);
            return Ok(new { message = $"ID'si {id} olan bakım kaydı başarıyla silindi." });
        }
    }
}
