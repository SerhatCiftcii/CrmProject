using AutoMapper;
using CrmProject.Application.DTOs.CustomerChangeLogDtos;
using CrmProject.Domain.Entities;

namespace CrmProject.Application.Mapping
{
    public class CustomerChangeLogProfile : Profile
    {
        public CustomerChangeLogProfile()
        {
            CreateMap<CustomerChangeLog, CustomerChangeLogDto>()
                .ForMember(dest => dest.CompanyName, opt => opt.MapFrom(src => src.Customer.CompanyName))
                .ForMember(dest => dest.BranchName, opt => opt.MapFrom(src => src.Customer.BranchName))
                .ForMember(dest => dest.OwnerName, opt => opt.MapFrom(src => src.Customer.OwnerName))
                .ForMember(dest => dest.ChangedBy, opt => opt.MapFrom(src => src.ChangedByUser.FullName));

            CreateMap<CreateCustomerChangeLogDto, CustomerChangeLog>();
        }
    }
}
