using AutoMapper;
using CrmProject.Application.Dtos.MaintenanceDtos;
using CrmProject.Application.DTOs.ProductDtos;
using CrmProject.Application.Helpers;
using CrmProject.Domain.Entities;

public class MaintenanceProfile : Profile
{
    public MaintenanceProfile()
    {
        //  Maintenance -> MaintenanceDetailDto
        CreateMap<Maintenance, MaintenanceDetailDto>()
              .ForMember(dest => dest.CompanyName, opt => opt.MapFrom(src => src.Customer.CompanyName))
              .ForMember(dest => dest.OfferStatus, opt => opt.MapFrom(src => EnumHelper.GetDisplayName(src.OfferStatus)))
              .ForMember(dest => dest.ContractStatus, opt => opt.MapFrom(src => EnumHelper.GetDisplayName(src.ContractStatus)))
              .ForMember(dest => dest.LicenseStatus, opt => opt.MapFrom(src => EnumHelper.GetDisplayName(src.LicenseStatus)))
              .ForMember(dest => dest.FirmSituation, opt => opt.MapFrom(src => EnumHelper.GetDisplayName(src.FirmSituation)))
              .ForMember(dest => dest.Products, opt => opt.MapFrom(src => src.MaintenanceProducts.Select(mp => mp.Product)));

        //  Product -> ProductDto (Maintenance içindeki ürünleri maplemek için)
        CreateMap<Product, ProductDto>();

        //  CreateMaintenanceDto -> Maintenance (Veri eklerken kullanılacak)
        CreateMap<CreateMaintenanceDto, Maintenance>()
            .ForMember(dest => dest.MaintenanceProducts, opt => opt.Ignore());
        // Ürün ekleme service tarafında yapılacak

        //  UpdateMaintenanceDto -> Maintenance (Veri güncellerken kullanılacak)
        CreateMap<UpdateMaintenanceDto, Maintenance>()
            .ForMember(dest => dest.MaintenanceProducts, opt => opt.Ignore());
        // Mevcut ürünler service’de temizlenip yeniden eklenecek
    }
}
