// CrmProject.Application/MappingProfiles/CustomerMappingProfile.cs

using AutoMapper;
using CrmProject.Application.DTOs.CustomerDtos;
using CrmProject.Domain.Entities;

namespace CrmProject.Application.MappingProfiles
{
    public class CustomerMappingProfile : Profile
    {
        public CustomerMappingProfile()
        {
            // Customer entity'sinden CustomerDto'ya eşleştirme
            CreateMap<Customer, CustomerDto>()
                .ForMember(dest => dest.Products, opt => opt.MapFrom(src =>
                    src.CustomerProducts.Select(cp => cp.Product).ToList()))
                .ReverseMap();

            // CreateCustomerDto'dan Customer entity'sine eşleştirme
            CreateMap<CreateCustomerDto, Customer>()
                .ForMember(dest => dest.CustomerProducts, opt => opt.Ignore());
            // Status eşlemesi otomatik yapılır (isimler aynıysa)

            // UpdateCustomerDto'dan Customer entity'sine eşleştirme
            CreateMap<UpdateCustomerDto, Customer>()
                .ForMember(dest => dest.CustomerProducts, opt => opt.Ignore());
            //  Status eşlemesi otomatik yapılır (isimler aynıysa)
        }
    }
}
