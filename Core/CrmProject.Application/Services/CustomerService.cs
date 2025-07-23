// CrmProject.Application/Services/CustomerService.cs

using AutoMapper; // IMapper için
using CrmProject.Application.DTOs; // DTO'lar için
using CrmProject.Application.Interfaces; // IUnitOfWork ve ICustomerService için
using CrmProject.Application.Validations; // CustomerValidator için
using CrmProject.Domain.Entities; // Customer entity'si için
using System.Collections.Generic;
using System.Threading.Tasks;
using FluentValidation; // IValidator için

namespace CrmProject.Application.Services
{
    public class CustomerService : ICustomerService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly CustomerValidator _customerValidator; // Entity doğrulayıcı

        public CustomerService(IUnitOfWork unitOfWork, IMapper mapper, CustomerValidator customerValidator)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _customerValidator = customerValidator;
        }

        public async Task<IEnumerable<CustomerDto>> GetAllCustomersAsync()
        {
            var customers = await _unitOfWork.GetRepository<Customer>().GetAllAsync();
            return _mapper.Map<IEnumerable<CustomerDto>>(customers);
        }

        public async Task<CustomerDto> GetCustomerByIdAsync(int id)
        {
            var customer = await _unitOfWork.GetRepository<Customer>().GetByIdAsync(id);
            return _mapper.Map<CustomerDto>(customer);
        }

        public async Task<CustomerDto> AddCustomerAsync(CreateCustomerDto createCustomerDto)
        {
            var customer = _mapper.Map<Customer>(createCustomerDto);

            // Entity'yi kaydetmeden önce doğrulama yap
            var validationResult = await _customerValidator.ValidateAsync(customer);
            if (!validationResult.IsValid)
            {
                // Doğrulama başarısız olursa hata fırlat
                throw new ValidationException(validationResult.Errors);
            }

            await _unitOfWork.GetRepository<Customer>().AddAsync(customer);
            await _unitOfWork.SaveChangesAsync();
            return _mapper.Map<CustomerDto>(customer);
        }

        public async Task UpdateCustomerAsync(UpdateCustomerDto updateCustomerDto)
        {
            var existingCustomer = await _unitOfWork.GetRepository<Customer>().GetByIdAsync(updateCustomerDto.Id);
            if (existingCustomer == null)
            {
                // Müşteri bulunamazsa hata fırlat veya özel bir yanıt döndür.
                // Burada basitçe null kontrolü yapıldı, daha sonra özel Exception'lar eklenebilir.
                throw new KeyNotFoundException($"Customer with ID {updateCustomerDto.Id} not found.");
            }

            // Mevcut entity üzerine DTO'daki değişiklikleri eşle
            _mapper.Map(updateCustomerDto, existingCustomer);

            // Güncellenen entity'yi kaydetmeden önce doğrulama yap
            var validationResult = await _customerValidator.ValidateAsync(existingCustomer);
            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors);
            }

            _unitOfWork.GetRepository<Customer>().Update(existingCustomer);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task DeleteCustomerAsync(int id)
        {
            var customer = await _unitOfWork.GetRepository<Customer>().GetByIdAsync(id);
            if (customer == null)
            {
                // Müşteri bulunamazsa hata fırlat veya özel bir yanıt döndür.
                throw new KeyNotFoundException($"Customer with ID {id} not found.");
            }

            _unitOfWork.GetRepository<Customer>().Remove(customer);
            await _unitOfWork.SaveChangesAsync();
        }
    }
}
