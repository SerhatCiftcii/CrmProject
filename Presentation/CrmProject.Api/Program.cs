// CrmProject.Api/Program.cs

using AutoMapper; // AutoMapper için eklendi
using CrmProject.Application.Common.Settings;
using CrmProject.Application.Interfaces;
using CrmProject.Application.MappingProfiles; // AutoMapper profilleri için eklendi
using CrmProject.Application.Services; // CustomerService için eklendi
using CrmProject.Application.Services.AuthorizedPersonServices;
using CrmProject.Application.Services.AuthServices;
using CrmProject.Application.Services.CustomerChangeLogServices;
using CrmProject.Application.Services.EmailServices;
using CrmProject.Application.Services.EmailServices.cs;
using CrmProject.Application.Services.MaintenanceServices; // MaintenanceService için eklendi
using CrmProject.Application.Services.ServiceProducts;
using CrmProject.Application.Settings;
using CrmProject.Application.Validations; // CustomerValidator için eklendi
using CrmProject.Domain.Entities;
using CrmProject.Infrastructure.Persistence.Context;
using CrmProject.Persistence.Repositories;
using CrmProject.Persistence.Repositories.IMaintenanceRepositories; //  MaintenanceRepository interface için eklendi
using CrmProject.Persistence.Repositories.MaintenanceRepositories; //  MaintenanceRepository implementasyonu için eklendi

using FluentValidation; // FluentValidation için eklendi
using FluentValidation.AspNetCore; // FluentValidation ASP.NET Core entegrasyonu için eklendi
using Microsoft.AspNetCore.Authentication.JwtBearer; // JWT Authentication için eklendi
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens; // Token validation için eklendi
using Microsoft.OpenApi.Models;
using System.Reflection; // Assembly taraması için eklendi
using System.Text; // Encoding için eklendi

var builder = WebApplication.CreateBuilder(args);

// CORS politikası için bir isim tanımlıyoruz
var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";

// AppDbContext'i konteynere ekliyoruz.
// AppDbContext kendi bağlantı dizesini OnConfiguring metodunda yönettiği için burada options.UseSqlServer() kullanmaya gerek yoktur.
builder.Services.AddDbContext<AppDbContext>();

// Identity servisleri ekleniyor
builder.Services.AddIdentity<AppUser, AppRole>(options =>
{
    options.Password.RequireDigit = false;
    options.Password.RequiredLength = 6;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = false;
    options.Password.RequireLowercase = false;
})
    .AddEntityFrameworkStores<AppDbContext>()
    .AddDefaultTokenProviders();

// JWT ayarlarını yapılandırma
builder.Services.Configure<JwtSettings>(
    builder.Configuration.GetSection("JwtSettings"));

// JWT Authentication yapılandırması (mutlaka eklenmeli)
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    // Token doğrulama parametreleri
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["JwtSettings:Issuer"],
        ValidAudience = builder.Configuration["JwtSettings:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JwtSettings:SecretKey"])),
        ClockSkew = TimeSpan.Zero // Token süresi hassas kontrol için (varsayılan 5 dk)
    };
});

// Repository ve Unit of Work servislerini konteynere ekliyoruz.
builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
builder.Services.AddScoped<ICustomerRepository, CustomerRepository>();
builder.Services.AddScoped<IAuthorizedPersonRepository, AuthorizedPersonRepository>();

//  Maintenance Repository kaydı
builder.Services.AddScoped<IMaintenanceRepository, MaintenanceRepository>();

builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<IProductRepository, ProductRepository>();

builder.Services.AddScoped<ICustomerChangeLogRepository, CustomerChangeLogRepository>();
builder.Services.AddScoped<ICustomerChangeLogService, CustomerChangeLogService>();
// AutoMapper servislerini ekliyoruz.
// Birden fazla assembly'deki tüm Profile sınıflarını otomatik olarak bulur ve kaydeder.
builder.Services.AddAutoMapper(new[] {
    Assembly.GetExecutingAssembly(),
    typeof(CustomerMappingProfile).Assembly,  // CustomerMappingProfile'ın bulunduğu assembly
    typeof(ProductMappingProfile).Assembly,   // ProductMappingProfile'ın bulunduğu assembly
    typeof(MaintenanceProfile).Assembly //  MaintenanceMappingProfile eklendi
});

// FluentValidation servislerini ekliyoruz.
// Bu, Controller'larda otomatik doğrulama için gereklidir.
builder.Services.AddFluentValidationAutoValidation();
// Validatörleri otomatik kaydet
builder.Services.AddValidatorsFromAssembly(typeof(CustomerValidator).Assembly);
builder.Services.AddValidatorsFromAssembly(typeof(ProductValidator).Assembly);  // ProductValidator'ın bulunduğu assembly

//  MaintenanceValidator kaydı
builder.Services.AddValidatorsFromAssembly(typeof(MaintenanceValidator).Assembly);
builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<AuthorizedPersonValidator>();

// Uygulama servislerini (Business Logic) konteynere ekliyoruz.
builder.Services.AddScoped<ICustomerService, CustomerService>();
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<IAuthorizedPersonService, AuthorizedPersonService>();

//  MaintenanceService kaydı
builder.Services.AddScoped<IMaintenanceService, MaintenanceService>();

// AuthService ve JWT token generator servislerini ekliyoruz.
builder.Services.AddScoped<IJwtTokenGenerator, JwtTokenGenerator>();
builder.Services.AddScoped<IAuthService, AuthService>();

// Email settings configuration
builder.Services.Configure<EmailSettings>(builder.Configuration.GetSection("EmailSettings"));
builder.Services.AddScoped<IEmailService, EmailService>();

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the Bearer scheme. Example: 'Bearer 12345abcdef'",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                },
                Scheme = "Bearer",
                Name = "Authorization",
                In = ParameterLocation.Header
            },
            new List<string>()
        }
    });
});

// CORS servisi
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: MyAllowSpecificOrigins,
                      builder =>
                      {
                          builder.AllowAnyOrigin() // Herhangi bir kaynaktan gelen isteklere izin verir (geliştirme ortamı için uygun)
                                 .AllowAnyHeader() // Herhangi bir HTTP başlığına izin verir
                                 .AllowAnyMethod(); // Herhangi bir HTTP metoduna (GET, POST, PUT, DELETE) izin verir
                      });
});

var app = builder.Build();
// Burada await kullanabiliriz çünkü minimal API'de async Main gibi davranır.
await SeedRolesAndUsersAsync(app.Services);

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication(); // Mutlaka UseAuthorization'dan önce olacak

app.UseAuthorization();

app.MapControllers();

app.Run();

async Task SeedRolesAndUsersAsync(IServiceProvider services)
{
    using var scope = services.CreateScope();
    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<AppRole>>();
    var userManager = scope.ServiceProvider.GetRequiredService<UserManager<AppUser>>();

    await SeedData.SeedRolesAsync(roleManager);
    await SeedData.SeedSuperAdminUserAsync(userManager);
    await SeedData.SeedAdminUserAsync(userManager);
}
