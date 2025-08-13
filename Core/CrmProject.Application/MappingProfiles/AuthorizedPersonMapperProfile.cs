// Dosya Yolu: CrmProject.Application/Mapping/AuthorizedPersonMapperProfile.cs

using AutoMapper;
using CrmProject.Application.Dtos.AuthorizedPersonDtos;
using CrmProject.Domain.Entities;

namespace CrmProject.Application.Mapping
{
    // AutoMapper'ın profil sınıfı, entity ve DTO'lar arasındaki dönüşümleri tanımlar.
    public class AuthorizedPersonMapperProfile : Profile
    {
        public AuthorizedPersonMapperProfile()
        {
            CreateMap<AuthorizedPerson, AuthorizedPersonDto>().ReverseMap();
                    

            // CreateAuthorizedPersonDto'dan AuthorizedPerson entity'sine dönüşüm kuralı.
            // Bu, kullanıcıdan gelen yeni yetkili kişi bilgilerinin veritabanına kaydedilecek hale getirilmesini sağlar.
            CreateMap<CreateAuthorizedPersonDto, AuthorizedPerson>();

            // UpdateAuthorizedPersonDto'dan AuthorizedPerson entity'sine dönüşüm kuralı.
            // Bu, kullanıcıdan gelen güncel yetkili kişi bilgilerinin mevcut entity üzerine uygulanmasını sağlar.
            CreateMap<UpdateAuthorizedPersonDto, AuthorizedPerson>();
        }
    }
}
