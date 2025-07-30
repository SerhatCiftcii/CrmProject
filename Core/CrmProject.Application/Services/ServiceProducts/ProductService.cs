using AutoMapper;
using CrmProject.Application.DTOs.ProductDtos;
using CrmProject.Application.Interfaces;
using CrmProject.Application.Validations;
using CrmProject.Domain.Entities;
using FluentValidation;

namespace CrmProject.Application.Services.ServiceProducts
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepo;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ProductValidator _validator;

        public ProductService(IProductRepository productRepo,
                              IUnitOfWork unitOfWork,
                              IMapper mapper,
                              ProductValidator validator)
        {
            _productRepo = productRepo;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _validator = validator;
        }

        public async Task<IEnumerable<ProductDto>> GetAllProductsAsync()
        {
            var products = _productRepo.GetAll().ToList();
            return _mapper.Map<IEnumerable<ProductDto>>(products);
        }

        public async Task<ProductDto> GetProductByIdAsync(int id)
        {
            var product = await _productRepo.GetByIdAsync(id);
            return _mapper.Map<ProductDto>(product);
        }

        public async Task<ProductDto> AddProductAsync(CreateProductDto dto)
        {
            var product = _mapper.Map<Product>(dto);

            var validation = await _validator.ValidateAsync(product);
            if (!validation.IsValid)
                throw new ValidationException(validation.Errors);

            await _productRepo.AddAsync(product);
            await _unitOfWork.SaveChangesAsync();

            return _mapper.Map<ProductDto>(product);
        }

        public async Task UpdateProductAsync(UpdateProductDto dto)
        {
            var product = await _productRepo.GetByIdAsync(dto.Id);
            if (product == null)
                throw new KeyNotFoundException($"ID'si {dto.Id} olan ürün bulunamadı.");

            _mapper.Map(dto, product);

            var validation = await _validator.ValidateAsync(product);
            if (!validation.IsValid)
                throw new ValidationException(validation.Errors);

            _productRepo.Update(product);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task DeleteProductAsync(int id)
        {
            var product = await _productRepo.GetByIdAsync(id);
            if (product != null)
            {
                _productRepo.Delete(product);
                await _unitOfWork.SaveChangesAsync();
            }
        }
    }
}
