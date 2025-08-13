using CrmProject.Application.Dtos.AuthorizedPersonDtos;
using CrmProject.Application.Services.AuthorizedPersonServices;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CrmProject.WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthorizedPersonController : ControllerBase
    {
        private readonly IAuthorizedPersonService _authorizedPersonService;

        public AuthorizedPersonController(IAuthorizedPersonService authorizedPersonService)
        {
            _authorizedPersonService = authorizedPersonService;
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var list = await _authorizedPersonService.GetAllAuthorizedPersonAsync();
            return Ok(list);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var authorizedPerson = await _authorizedPersonService.GetAuthorizedPersonByIdAsync(id);
            if (authorizedPerson == null)
                return NotFound(new { message = $"ID'si {id} olan yetkili kişi bulunamadı." });

            return Ok(authorizedPerson);
        }

        [HttpPost]
        [Authorize(Roles = "Admin,SuperAdmin")]
        public async Task<IActionResult> Create([FromBody] CreateAuthorizedPersonDto dto)
        {
            try
            {
                var created = await _authorizedPersonService.AddAuthorizedPersonAsync(dto);
                return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
            }
            catch (ValidationException ex)
            {
                return BadRequest(new { message = ex.Message, errors = ex.Errors });
            }
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin,SuperAdmin")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateAuthorizedPersonDto dto)
        {
            if (id != dto.Id)
                return BadRequest(new { message = "URL'deki ID ile body'deki ID eşleşmiyor." });

            try
            {
                await _authorizedPersonService.UpdateAuthorizedPersonAsync(dto);
                return Ok(new { message = $"ID'si {id} olan yetkili kişi başarıyla güncellendi." });
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (ValidationException ex)
            {
                return BadRequest(new { message = ex.Message, errors = ex.Errors });
            }
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin,SuperAdmin")]
        public async Task<IActionResult> Delete(int id)
        {
            // Önce ürünün varlığını kontrol et
            var existingProduct = await _authorizedPersonService.GetAuthorizedPersonByIdAsync(id);
            if (existingProduct == null)
            {
                return NotFound(new { message = $"ID'si {id} olan ürün bulunamadı. Silme yapılamadı." });
            }
            await _authorizedPersonService.DeleteAuthorizedPersonAsync(id);
            return Ok(new { message = $"ID'si {id} olan yetkili kişi başarıyla silindi." });
        }
    }
}
