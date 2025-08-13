using CrmProject.Application.Dtos.AuthDtos;
using CrmProject.Application.Services.AuthServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CrmProject.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDto dto)
        {
            var token = await _authService.LoginAsync(dto);
            if (token == null) return Unauthorized("Geçersiz kullanıcı adı, şifre veya kullanıcı pasif.");
            return Ok(new { Token = token });
        }

        [AllowAnonymous]
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequestDto dto)
        {
            var result = await _authService.RegisterAsync(dto);
            if (!result) return BadRequest("Kullanıcı zaten mevcut veya kayıt başarısız.");
            return Ok("Kayıt başarılı, SuperAdmin tarafından onaylanmayı bekliyor.");
        }

        [Authorize(Roles = "SuperAdmin")]
        [HttpPost("admin/add")]
        public async Task<IActionResult> AddAdmin([FromBody] AddAdminDto dto)
        {
            var result = await _authService.AddAdminAsync(dto);
            if (!result) return BadRequest("Admin ekleme başarısız, kullanıcı zaten mevcut olabilir.");
            return Ok("Admin başarıyla eklendi.");
        }

        [Authorize(Roles = "SuperAdmin")]
        [HttpPost("admin/setstatus")]
        public async Task<IActionResult> SetActiveStatus([FromBody] AdminStatusUpdateDto dto)
        {
            var result = await _authService.SetActiveStatusAsync(dto);
            if (!result) return BadRequest("Durum güncelleme başarısız.");
            return Ok("Durum başarıyla güncellendi.");
        }

        [Authorize(Roles = "Admin,SuperAdmin")]
        [HttpGet("admin/list")]
        public async Task<IActionResult> GetAllAdmins()
        {
            var list = await _authService.GetAllAdminsAsync();
            return Ok(list);
        }
        [Authorize] // Sadece giriş yapmış kullanıcılar erişebilir
        [HttpDelete("admin/delete/{id}")]
        public async Task<IActionResult> DeleteAdmin(string id)
        {
            var currentUserId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            var message = await _authService.DeleteAdminAsync(id, currentUserId);

            if (message.Contains("bulunamadı"))
                return NotFound(new { message });

            if (message.Contains("kendi hesabınızı silemez"))
                return BadRequest(new { message });

            if (message.Contains("Yetkiniz yok"))
                return StatusCode(StatusCodes.Status403Forbidden, new { message }); // Mesajlı 403

            if (message.Contains("başarısız"))
                return BadRequest(new { message });

            return Ok(new { message });
        }

    }
}
