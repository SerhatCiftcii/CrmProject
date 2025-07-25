using AutoMapper;
using CrmProject.Application.DTOs.ProductDtos;
using CrmProject.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrmProject.Application.MappingProfiles
{
    public class ProductMappingProfile : Profile
    {
        public ProductMappingProfile()
        {
            // Product entity'sinden ProductDto'ya ve tersine eşleştirme
            CreateMap<Product, ProductDto>().ReverseMap();

            // CreateProductDto'dan Product entity'sine eşleştirme
            CreateMap<CreateProductDto, Product>();

            // UpdateProductDto'dan Product entity'sine eşleştirme
            CreateMap<UpdateProductDto, Product>();
        }
    }
}
