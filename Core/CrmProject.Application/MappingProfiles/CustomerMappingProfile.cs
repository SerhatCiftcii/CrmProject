// CrmProject.Application/MappingProfiles/CustomerMappingProfile.cs

using AutoMapper;
using CrmProject.Application.DTOs;
using CrmProject.Domain.Entities;

namespace CrmProject.Application.MappingProfiles
{
    public class CustomerMappingProfile : Profile
    {
        public CustomerMappingProfile()
        {
            // Customer entity'sinden CustomerDto'ya ve tersine eşleştirme
            CreateMap<Customer, CustomerDto>().ReverseMap();

            // CreateCustomerDto'dan Customer entity'sine eşleştirme
            CreateMap<CreateCustomerDto, Customer>();

            // UpdateCustomerDto'dan Customer entity'sine eşleştirme
            CreateMap<UpdateCustomerDto, Customer>();
        }
    }
}
