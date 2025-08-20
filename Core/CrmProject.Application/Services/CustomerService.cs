using AutoMapper;
using CrmProject.Application.DTOs.CustomerChangeLogDtos;
using CrmProject.Application.DTOs.CustomerDtos;
using CrmProject.Application.Interfaces;
using CrmProject.Application.Services.ServiceProducts;
using CrmProject.Application.Validations;
using CrmProject.Domain.Entities;
using FluentValidation;
using Microsoft.AspNetCore.Http;

namespace CrmProject.Application.Services
{
    public class CustomerService : ICustomerService
    {
        private readonly ICustomerRepository _customerRepo;
        private readonly IGenericRepository<Product> _productRepo;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly CustomerValidator _validator;
        private readonly IHttpContextAccessor _httpContextAccessor; // Token’dan userId almak için

        public CustomerService(ICustomerRepository customerRepo,
                               IGenericRepository<Product> productRepo,
                               IUnitOfWork unitOfWork,
                               IMapper mapper,
                               CustomerValidator validator, IHttpContextAccessor httpContextAccessor)
        {
            _customerRepo = customerRepo;
            _productRepo = productRepo;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _validator = validator;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<IEnumerable<CustomerDto>> GetAllCustomersAsync()
        {
            var customers = await _customerRepo.GetAllWithProductsAsync();
            return _mapper.Map<IEnumerable<CustomerDto>>(customers);
        }

        public async Task<CustomerDto> GetCustomerByIdAsync(int id)
        {
            var customer = await _customerRepo.GetCustomerWithProductsAsync(id);
            return _mapper.Map<CustomerDto>(customer);
        }

        public async Task<CustomerDto> AddCustomerAsync(CreateCustomerDto dto)
        {
            // Email kontrol
            if (_customerRepo.Where(c => c.Email == dto.Email).Any())
                throw new ValidationException("Bu email zaten kayıtlı.");

            var customer = _mapper.Map<Customer>(dto);
            var validation = await _validator.ValidateAsync(customer);
            if (!validation.IsValid)
                throw new ValidationException(validation.Errors);

            // Product kontrol
            if (dto.ProductIds?.Any() == true)
            {
                var existingIds = _productRepo.Where(p => dto.ProductIds.Contains(p.Id)).Select(p => p.Id).ToList();
                var missing = dto.ProductIds.Except(existingIds).ToList();
                if (missing.Any()) throw new ValidationException($"Geçersiz ürün ID: {string.Join(",", missing)}");

                customer.CustomerProducts = existingIds.Select(pid => new CustomerProduct
                {
                    ProductId = pid,
                    Customer = customer,
                    AssignedDate = DateTime.UtcNow
                }).ToList();
            }

            await _customerRepo.AddAsync(customer);
            await _unitOfWork.SaveChangesAsync();

            return _mapper.Map<CustomerDto>(customer);
        }

        public async Task UpdateCustomerAsync(UpdateCustomerDto dto, string userId)
        {
            // 1️ Müşteriyi çek
            var customer = await _customerRepo.GetCustomerWithProductsAsync(dto.Id)
                           ?? throw new KeyNotFoundException("Müşteri bulunamadı.");

            var logs = new List<CustomerChangeLog>();

            // 2️ Önemli alanları kontrol et ve log ekle
            if (customer.CompanyName != dto.CompanyName)
                logs.Add(new CustomerChangeLog
                {
                    CustomerId = customer.Id,
                    FieldName = "CompanyName",
                    OldValue = customer.CompanyName,
                    NewValue = dto.CompanyName,
                    ChangedByUserId = userId,
                    ChangedAt = DateTime.UtcNow
                });

            if (customer.BranchName != dto.BranchName)
                logs.Add(new CustomerChangeLog
                {
                    CustomerId = customer.Id,
                    FieldName = "BranchName",
                    OldValue = customer.BranchName,
                    NewValue = dto.BranchName,
                    ChangedByUserId = userId,
                    ChangedAt = DateTime.UtcNow
                });

            if (customer.OwnerName != dto.OwnerName)
                logs.Add(new CustomerChangeLog
                {
                    CustomerId = customer.Id,
                    FieldName = "OwnerName",
                    OldValue = customer.OwnerName,
                    NewValue = dto.OwnerName,
                    ChangedByUserId = userId,
                    ChangedAt = DateTime.UtcNow
                });

            // 3️ Customer entity’sini güncelle
            _mapper.Map(dto, customer);

            // 4️ Ürünleri güncelle
            customer.CustomerProducts.Clear();
            if (dto.ProductIds?.Any() == true)
            {
                foreach (var pid in dto.ProductIds)
                {
                    customer.CustomerProducts.Add(new CustomerProduct
                    {
                        CustomerId = customer.Id,
                        ProductId = pid,
                        AssignedDate = DateTime.UtcNow
                    });
                }
            }

            _customerRepo.Update(customer);

            // 5️ Logları ekle (her değişiklik ayrı satır)
            foreach (var log in logs)
            {
                await _unitOfWork.CustomerChangeLogs.AddAsync(log);
            }

            // 6️ Kaydet
            await _unitOfWork.SaveChangesAsync();
        }


        public async Task DeleteCustomerAsync(int id)
        {
            var customer = await _customerRepo.GetByIdAsync(id);
            if (customer != null)
            {
                _customerRepo.Delete(customer);
                await _unitOfWork.SaveChangesAsync();
            }
        }
    }
}
