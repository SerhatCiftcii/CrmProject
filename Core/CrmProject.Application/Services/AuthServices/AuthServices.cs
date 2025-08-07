using CrmProject.Application.Dtos.AuthDtos;
using CrmProject.Application.Dtos;
using CrmProject.Application.Interfaces;

using CrmProject.Application.Services.AuthServices;
using CrmProject.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

namespace CrmProject.Application.Services
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IJwtTokenGenerator _jwtTokenGenerator;

        public AuthService(UserManager<AppUser> userManager, IJwtTokenGenerator jwtTokenGenerator)
        {
            _userManager = userManager;
            _jwtTokenGenerator = jwtTokenGenerator;
        }

        public async Task<string> LoginAsync(LoginRequestDto dto)
        {
            var user = await _userManager.FindByNameAsync(dto.Username);
            if (user == null) return null;

            var isPasswordValid = await _userManager.CheckPasswordAsync(user, dto.Password);
            if (!isPasswordValid) return null;

            // Kullanıcının rollerini çek
            var roles = await _userManager.GetRolesAsync(user);

            // Token üretirken roller listesini de ver
            var token = _jwtTokenGenerator.GenerateToken(user, roles);

            return token;
        }
    }
}
