using AutoMapper;
using CrmProject.Application.DTOs.CustomerDtos;
using CrmProject.Application.Interfaces;
using CrmProject.Application.Services.ServiceProducts;
using CrmProject.Application.Validations;
using CrmProject.Domain.Entities;
using FluentValidation;

namespace CrmProject.Application.Services
{
    public class CustomerService : ICustomerService
    {
        private readonly ICustomerRepository _customerRepo;
        private readonly IGenericRepository<Product> _productRepo;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly CustomerValidator _validator;

        public CustomerService(ICustomerRepository customerRepo,
                               IGenericRepository<Product> productRepo,
                               IUnitOfWork unitOfWork,
                               IMapper mapper,
                               CustomerValidator validator)
        {
            _customerRepo = customerRepo;
            _productRepo = productRepo;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _validator = validator;
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

        public async Task UpdateCustomerAsync(UpdateCustomerDto dto)
        {
            var customer = await _customerRepo.GetCustomerWithProductsAsync(dto.Id)
                           ?? throw new KeyNotFoundException("Müşteri bulunamadı.");

            if (_customerRepo.Where(c => c.Email == dto.Email && c.Id != dto.Id).Any())
                throw new ValidationException("Bu email başka bir müşteri tarafından kullanılıyor.");

            _mapper.Map(dto, customer);

            var validation = await _validator.ValidateAsync(customer);
            if (!validation.IsValid)
                throw new ValidationException(validation.Errors);

            // Eski ürünleri temizle + yenileri ekle
            customer.CustomerProducts.Clear();
            if (dto.ProductIds?.Any() == true)
            {
                var existingIds = _productRepo.Where(p => dto.ProductIds.Contains(p.Id)).Select(p => p.Id).ToList();
                foreach (var pid in existingIds)
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
