using AutoMapper;
using CrmProject.Application.DTOs.CustomerDtos;
using CrmProject.Application.Interfaces;
using CrmProject.Application.Services.ServiceProducts;
using CrmProject.Application.Validations;
using CrmProject.Domain.Entities;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CrmProject.Application.Services
{
    public class CustomerService : ICustomerService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly CustomerValidator _customerValidator;

        public CustomerService(IUnitOfWork unitOfWork, IMapper mapper, CustomerValidator customerValidator)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _customerValidator = customerValidator;
        }

        //  TÜM MÜŞTERİLERİ ÜRÜNLERİYLE GETİRİR
        public async Task<IEnumerable<CustomerDto>> GetAllCustomersAsync()
        {
            var customers = await _unitOfWork
                .GetRepository<Customer>()
                .Query()
                .Include(c => c.CustomerProducts)      // CustomerProducts ilişkisini dahil et
                .ThenInclude(cp => cp.Product)         // Her CustomerProduct içindeki Product'ı dahil et
                .ToListAsync();

            return _mapper.Map<IEnumerable<CustomerDto>>(customers);
        }

        //  ID'YE GÖRE MÜŞTERİ + ÜRÜNLERİ GETİRİR
        public async Task<CustomerDto> GetCustomerByIdAsync(int id)
        {
            var customer = await _unitOfWork
                .GetRepository<Customer>()
                .Query()
                .Include(c => c.CustomerProducts)
                .ThenInclude(cp => cp.Product)
                .FirstOrDefaultAsync(c => c.Id == id);

            return _mapper.Map<CustomerDto>(customer);
        }

        //  YENİ MÜŞTERİ EKLER (ÜRÜN ID DOĞRULAMALI)
        public async Task<CustomerDto> AddCustomerAsync(CreateCustomerDto createCustomerDto)
        {
            var customer = _mapper.Map<Customer>(createCustomerDto);

            //  Önce aynı email var mı kontrol et
            var emailExists = await _unitOfWork.GetRepository<Customer>()
                .Query()
                .AnyAsync(c => c.Email == createCustomerDto.Email);
            if (emailExists)
                throw new ValidationException("Bu email adresi zaten kayıtlı.");

            //  Customer alanlarını doğrula
            var validationResult = await _customerValidator.ValidateAsync(customer);
            if (!validationResult.IsValid)
                throw new ValidationException(validationResult.Errors);

            //  Product ID kontrolü yap
            var productIds = createCustomerDto.ProductIds?.Distinct().ToList() ?? new List<int>();
            if (productIds.Any())
            {
                var productRepo = _unitOfWork.GetRepository<Product>();
                var existingProducts = await productRepo.GetWhereAsync(p => productIds.Contains(p.Id));
                var existingProductIds = existingProducts.Select(p => p.Id).ToList();

                //  Geçersiz ID'ler varsa hata fırlat
                var missingIds = productIds.Except(existingProductIds).ToList();
                if (missingIds.Any())
                    throw new ValidationException($"Şu ID'lere sahip ürün(ler) bulunamadı: {string.Join(",", missingIds)}");

                //  İlişkileri oluştur
                customer.CustomerProducts = existingProductIds
                    .Select(pid => new CustomerProduct
                    {
                        ProductId = pid,
                        Customer = customer,
                        AssignedDate = DateTime.UtcNow
                    }).ToList();
            }

            //  DB'ye kaydet
            await _unitOfWork.GetRepository<Customer>().AddAsync(customer);
            await _unitOfWork.SaveChangesAsync();

            return _mapper.Map<CustomerDto>(customer);
        }


        //  MÜŞTERİ GÜNCELLER (ÜRÜN ID DOĞRULAMALI)
        public async Task UpdateCustomerAsync(UpdateCustomerDto updateCustomerDto)
        {
            var customerRepo = _unitOfWork.GetRepository<Customer>();

            //  Müşteriyi ilişkili ürünlerle yükle
            var existingCustomer = await customerRepo.GetByIdWithIncludesAsync(updateCustomerDto.Id, c => c.CustomerProducts);
            if (existingCustomer == null)
                throw new KeyNotFoundException($"Customer with ID {updateCustomerDto.Id} not found.");

            //  Aynı email başka müşteri tarafından kullanılıyor mu kontrol et
            var emailExists = await _unitOfWork.GetRepository<Customer>()
                .Query()
                .AnyAsync(c => c.Email == updateCustomerDto.Email && c.Id != updateCustomerDto.Id);
            if (emailExists)
                throw new ValidationException("Bu email adresi başka bir müşteri tarafından kullanılıyor.");

            //  DTO'dan gelen alanları map et
            _mapper.Map(updateCustomerDto, existingCustomer);

            //  Customer alanlarını doğrula
            var validationResult = await _customerValidator.ValidateAsync(existingCustomer);
            if (!validationResult.IsValid)
                throw new ValidationException(validationResult.Errors);

            //  Eski ürün ilişkilerini temizle
            existingCustomer.CustomerProducts.Clear();

            //  Yeni ürün ID'lerini kontrol et
            var productIds = updateCustomerDto.ProductIds?.Distinct().ToList() ?? new List<int>();
            if (productIds.Any())
            {
                var productRepo = _unitOfWork.GetRepository<Product>();
                var existingProducts = await productRepo.GetWhereAsync(p => productIds.Contains(p.Id));
                var existingProductIds = existingProducts.Select(p => p.Id).ToList();

                // ❗ Geçersiz ID'ler varsa hata fırlat
                var missingIds = productIds.Except(existingProductIds).ToList();
                if (missingIds.Any())
                    throw new ValidationException($"Şu ID'lere sahip ürün(ler) bulunamadı: {string.Join(",", missingIds)}");

                // ✔️ Yeni ilişkileri ekle
                foreach (var pid in existingProductIds)
                {
                    existingCustomer.CustomerProducts.Add(new CustomerProduct
                    {
                        CustomerId = existingCustomer.Id,
                        ProductId = pid,
                        AssignedDate = DateTime.UtcNow
                    });
                }
            }

            //  DB'ye kaydet
            customerRepo.Update(existingCustomer);
            await _unitOfWork.SaveChangesAsync();
        }

        //  MÜŞTERİ SİLER
        public async Task DeleteCustomerAsync(int id)
        {
            var customerRepo = _unitOfWork.GetRepository<Customer>();
            var customer = await customerRepo.GetByIdAsync(id);
            if (customer != null)
            {
                customerRepo.Remove(customer);
                await _unitOfWork.SaveChangesAsync();
            }
        }
    }
}
