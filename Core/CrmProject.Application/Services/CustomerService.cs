// CrmProject.Application/Services/Customer/CustomerService.cs

using AutoMapper; // IMapper için
using CrmProject.Application.DTOs.CustomerDtos; // DTO'lar için
using CrmProject.Application.Interfaces; // IUnitOfWork ve ICustomerService için
using CrmProject.Application.Services.ServiceProducts;
using CrmProject.Application.Validations; // CustomerValidator için
using CrmProject.Domain.Entities; // Customer entity'si için
using FluentValidation; // ValidationException için
using System; // KeyNotFoundException için
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

// Not: CrmProject.Application.Services.ServiceProducts using'i CustomerService içinde gerekli değildir.
// using CrmProject.Application.Services.ServiceProducts;

namespace CrmProject.Application.Services // Namespace güncellendi (Customer klasörü içindeki servis için)
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
                // Müşteri bulunamazsa KeyNotFoundException fırlatırız.
                // Bu, Controller'da yakalanarak 404 Not Found yanıtına dönüşecektir.
                throw new KeyNotFoundException($"Customer with ID {updateCustomerDto.Id} not found.");
            }

            // Mevcut entity üzerine DTO'daki değişiklikleri eşle
            _mapper.Map(updateCustomerDto, existingCustomer);

            // Güncellenen entity'yi kaydetmeden önce doğrulama yap
            var validationResult = await _customerValidator.ValidateAsync(existingCustomer);
            if (!validationResult.IsValid)
            {
                // Doğrulama başarısız olursa hata fırlat
                throw new ValidationException(validationResult.Errors);
            }

            _unitOfWork.GetRepository<Customer>().Update(existingCustomer);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task DeleteCustomerAsync(int id)
        {
            // Servis burada KeyNotFoundException fırlatmayacak.
            // Controller zaten varlık kontrolünü yapacak.
            var customer = await _unitOfWork.GetRepository<Customer>().GetByIdAsync(id);
            if (customer != null) // Sadece müşteri bulunursa silme işlemini yap
            {
                _unitOfWork.GetRepository<Customer>().Remove(customer);
                await _unitOfWork.SaveChangesAsync();
            }
        }
    }
}
