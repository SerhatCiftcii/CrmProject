using CrmProject.Application.Dtos.AuthDtos;
using CrmProject.Application.Interfaces;
using CrmProject.Application.Services.AuthServices;
using CrmProject.Application.Validations.AuthValidator;
using CrmProject.Domain.Entities;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

public class AuthService : IAuthService
{
    private readonly UserManager<AppUser> _userManager;
    private readonly IJwtTokenGenerator _jwtTokenGenerator;
    private readonly IValidator<RegisterRequestDto> _registerValidator;
    private readonly IValidator<AddAdminDto> _addAdminValidator;

    public AuthService(
        UserManager<AppUser> userManager,
        IJwtTokenGenerator jwtTokenGenerator,
        IValidator<RegisterRequestDto> registerValidator,
        IValidator<AddAdminDto> addAdminValidator)
    {
        _userManager = userManager;
        _jwtTokenGenerator = jwtTokenGenerator;
        _registerValidator = registerValidator;
        _addAdminValidator = addAdminValidator;
    }

    public async Task<string> LoginAsync(LoginRequestDto dto)
    {
        var user = await _userManager.FindByNameAsync(dto.Username);
        if (user == null || !user.IsActive) return null;

        var isPasswordValid = await _userManager.CheckPasswordAsync(user, dto.Password);
        if (!isPasswordValid) return null;

        var roles = await _userManager.GetRolesAsync(user);
        return _jwtTokenGenerator.GenerateToken(user, roles);
    }

    public async Task<bool> RegisterAsync(RegisterRequestDto dto)
    {
        var validationResult = await _registerValidator.ValidateAsync(dto);
        if (!validationResult.IsValid)
            throw new ValidationException(validationResult.Errors);

        var existingUser = await _userManager.FindByNameAsync(dto.Username);
        if (existingUser != null) return false;

        var user = new AppUser
        {
            UserName = dto.Username,
            Email = dto.Email,
            FullName = dto.FullName,
            PhoneNumber = dto.PhoneNumber,
            IsActive = false,
            IsSuperAdmin = false
        };

        var result = await _userManager.CreateAsync(user, dto.Password);
        if (!result.Succeeded) return false;

        await _userManager.AddToRoleAsync(user, "Admin");
        return true;
    }

    public async Task<bool> AddAdminAsync(AddAdminDto dto)
    {
        var validationResult = await _addAdminValidator.ValidateAsync(dto);
        if (!validationResult.IsValid)
            throw new ValidationException(validationResult.Errors);

        var existingUser = await _userManager.FindByNameAsync(dto.Username);
        if (existingUser != null) return false;

        var user = new AppUser
        {
            UserName = dto.Username,
            Email = dto.Email,
            FullName = dto.FullName,
            PhoneNumber = dto.PhoneNumber,
            IsActive = true,
            IsSuperAdmin = false
        };

        var result = await _userManager.CreateAsync(user, dto.Password);
        if (!result.Succeeded) return false;

        await _userManager.AddToRoleAsync(user, "Admin");
        return true;
    }

    public async Task<bool> SetActiveStatusAsync(AdminStatusUpdateDto dto)
    {
        var user = await _userManager.FindByIdAsync(dto.UserId);
        if (user == null || user.IsSuperAdmin) return false;

        user.IsActive = dto.IsActive;
        await _userManager.UpdateAsync(user);
        return true;
    }

    public async Task<List<AdminListDto>> GetAllAdminsAsync()
    {
        var users = _userManager.Users
            .Select(u => new AdminListDto
            {
                Id = u.Id,
                Username = u.UserName,
                Email = u.Email,
                FullName = u.FullName,
                PhoneNumber = u.PhoneNumber,
                IsActive = u.IsActive,
                IsSuperAdmin = u.IsSuperAdmin
            });

        return await users.ToListAsync();
    }
    public async Task<string> DeleteAdminAsync(string targetUserId, string currentUserId)
    {
        var currentUser = await _userManager.FindByIdAsync(currentUserId);
        var targetUser = await _userManager.FindByIdAsync(targetUserId);

        if (targetUser == null)
            return "Kullanıcı bulunamadı";

        if (currentUserId == targetUserId)
            return "Kendi hesabınızı silemezsiniz";

        // SuperAdmin değilse yetki yok
        if (!await _userManager.IsInRoleAsync(currentUser, "SuperAdmin"))
            return "Yetkiniz yok";

        var result = await _userManager.DeleteAsync(targetUser);
        if (!result.Succeeded)
            return "Silme işlemi başarısız";

        return "Kullanıcı başarıyla silindi";
    }

}
