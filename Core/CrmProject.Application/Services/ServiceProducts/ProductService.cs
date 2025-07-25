// CrmProject.Application/Services/ServiceProducts/ProductService.cs

using AutoMapper;
using CrmProject.Application.DTOs.ProductDtos;
using CrmProject.Application.Interfaces;
using CrmProject.Application.Validations;
using FluentValidation;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using CrmProject.Domain.Entities; // Product entity'si için

namespace CrmProject.Application.Services.ServiceProducts
{
    public class ProductService : IProductService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ProductValidator _productValidator;

        public ProductService(IUnitOfWork unitOfWork, IMapper mapper, ProductValidator productValidator)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _productValidator = productValidator;
        }

        public async Task<IEnumerable<ProductDto>> GetAllProductsAsync()
        {
            var products = await _unitOfWork.GetRepository<Product>().GetAllAsync();
            return _mapper.Map<IEnumerable<ProductDto>>(products);
        }

        public async Task<ProductDto> GetProductByIdAsync(int id)
        {
            var product = await _unitOfWork.GetRepository<Product>().GetByIdAsync(id);
            return _mapper.Map<ProductDto>(product);
        }

        public async Task<ProductDto> AddProductAsync(CreateProductDto createProductDto)
        {
            var product = _mapper.Map<Product>(createProductDto);

            var validationResult = await _productValidator.ValidateAsync(product);
            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors);
            }

            await _unitOfWork.GetRepository<Product>().AddAsync(product);
            await _unitOfWork.SaveChangesAsync();
            return _mapper.Map<ProductDto>(product);
        }

        public async Task UpdateProductAsync(UpdateProductDto updateProductDto)
        {
            var existingProduct = await _unitOfWork.GetRepository<Product>().GetByIdAsync(updateProductDto.Id);
            if (existingProduct == null)
            {
                // Müşteri bulunamazsa KeyNotFoundException fırlatırız.
                // Controller'da yakalanmayacağı için 500 hatası dönecektir.
                throw new KeyNotFoundException($"ID'si {updateProductDto.Id} olan ürün bulunamadı.");
            }

            _mapper.Map(updateProductDto, existingProduct);

            var validationResult = await _productValidator.ValidateAsync(existingProduct);
            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors);
            }

            _unitOfWork.GetRepository<Product>().Update(existingProduct);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task DeleteProductAsync(int id)
        {
            // Servis burada KeyNotFoundException fırlatmayacak.
            // Controller zaten varlık kontrolünü yapacak.
            var product = await _unitOfWork.GetRepository<Product>().GetByIdAsync(id);
            if (product != null)
            {
                _unitOfWork.GetRepository<Product>().Remove(product);
                await _unitOfWork.SaveChangesAsync();
            }
        }
    }
}
