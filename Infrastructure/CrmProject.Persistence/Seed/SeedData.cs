using CrmProject.Domain.Entities;
using Microsoft.AspNetCore.Identity;

public static class SeedData
{
    public static async Task SeedRolesAsync(RoleManager<AppRole> roleManager)
    {
        if (!await roleManager.RoleExistsAsync("SuperAdmin"))
        {
            await roleManager.CreateAsync(new AppRole
            {
                Name = "SuperAdmin",
                NormalizedName = "SUPERADMIN",
                Description = "Sistemde tam yetkiye sahip kullanıcı"
            });
        }

        if (!await roleManager.RoleExistsAsync("Admin"))
        {
            await roleManager.CreateAsync(new AppRole
            {
                Name = "Admin",
                NormalizedName = "ADMIN",
                Description = "Yönetici rolü"
            });
        }
    }

    public static async Task SeedSuperAdminUserAsync(UserManager<AppUser> userManager)
    {
        var user = await userManager.FindByNameAsync("superadmin");
        if (user == null)
        {
            var superAdmin = new AppUser
            {
                UserName = "superadmin",
                Email = "superadmin@crm.com",
                FullName = "Süper Admin",
                EmailConfirmed = true,
                IsActive = true,
                IsSuperAdmin = true,
            };

            var result = await userManager.CreateAsync(superAdmin, "SuperAdmin123!");
            if (result.Succeeded)
            {
                await userManager.AddToRoleAsync(superAdmin, "SuperAdmin");
            }
        }
    }

    public static async Task SeedAdminUserAsync(UserManager<AppUser> userManager)
    {
        var user = await userManager.FindByNameAsync("admin");
        if (user == null)
        {
            var admin = new AppUser
            {
                UserName = "admin",
                Email = "admin@crm.com",
                FullName = "Normal Yönetici",
                EmailConfirmed = true,
                IsActive = true,
                IsSuperAdmin = false,
            };

            var result = await userManager.CreateAsync(admin, "Admin123!");
            if (result.Succeeded)
            {
                await userManager.AddToRoleAsync(admin, "Admin");
            }
        }
    }
}
