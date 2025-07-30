// CrmProject.Api/Program.cs

using AutoMapper; // AutoMapper için eklendi
using CrmProject.Application.Interfaces;
using CrmProject.Application.MappingProfiles; // AutoMapper profilleri için eklendi
using CrmProject.Application.Services; // CustomerService için eklendi
using CrmProject.Application.Services.ServiceProducts;
using CrmProject.Application.Validations; // CustomerValidator için eklendi
using CrmProject.Infrastructure.Persistence.Context;

using CrmProject.Persistence.Repositories;
using FluentValidation; // FluentValidation için eklendi
using FluentValidation.AspNetCore; // FluentValidation ASP.NET Core entegrasyonu için eklendi
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Reflection; // Assembly taraması için eklendi

var builder = WebApplication.CreateBuilder(args);

// CORS politikası için bir isim tanımlıyoruz
var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";
// AppDbContext'i konteynere ekliyoruz.
// AppDbContext kendi bağlantı dizesini OnConfiguring metodunda yönettiği için burada options.UseSqlServer() kullanmaya gerek yoktur.
builder.Services.AddDbContext<AppDbContext>();

// Repository ve Unit of Work servislerini konteynere ekliyoruz.
builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
builder.Services.AddScoped<ICustomerRepository, CustomerRepository>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<IProductRepository, ProductRepository>();
// AutoMapper servislerini ekliyoruz.
// Birden fazla assembly'deki tüm Profile sınıflarını otomatik olarak bulur ve kaydeder.
builder.Services.AddAutoMapper(new[] {
   Assembly.GetExecutingAssembly(),
        typeof(CustomerMappingProfile).Assembly, // CustomerMappingProfile'ın bulunduğu assembly
        typeof(ProductMappingProfile).Assembly   // ProductMappingProfile'ın bulunduğu assembly
    });


// FluentValidation servislerini ekliyoruz.
// Bu, Controller'larda otomatik doğrulama için gereklidir.
builder.Services.AddFluentValidationAutoValidation();
// Validatörleri otomatik kaydet
builder.Services.AddValidatorsFromAssembly(typeof(CustomerValidator).Assembly);
builder.Services.AddValidatorsFromAssembly(typeof(ProductValidator).Assembly);  // ProductValidator'ın bulunduğu assembly

// Uygulama servislerini (Business Logic) konteynere ekliyoruz.
builder.Services.AddScoped<ICustomerService, CustomerService>();
builder.Services.AddScoped<IProductService, ProductService>();

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
// CORS servisini ekliyoruz
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

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();
