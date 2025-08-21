using AutoMapper;
using CrmProject.Application.Dtos.MaintenanceDtos;
using CrmProject.Application.Helpers;
using CrmProject.Application.Interfaces;
using CrmProject.Application.Services.EmailServices;
using CrmProject.Application.Validations;
using CrmProject.Domain.Entities;
using CrmProject.Persistence.Repositories.IMaintenanceRepositories;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrmProject.Application.Services.MaintenanceServices
{
    public class MaintenanceService : IMaintenanceService
    {
        private readonly IMaintenanceRepository _maintanenceRepo;
        private readonly IGenericRepository<Product> _productRepository;
        IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IEmailService _emailService;

        private readonly MaintenanceValidator _validator;

        public MaintenanceService(IMaintenanceRepository maintanenceRepo, IUnitOfWork unitOfWork, IMapper mapper, MaintenanceValidator validator, IGenericRepository<Product> productRepository, IEmailService emailService)
        {
            _maintanenceRepo = maintanenceRepo;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _validator = validator;
            _productRepository = productRepository;
             _emailService = emailService;
        }

        public async Task<List<MaintenanceDetailDto>> GetAllMaintenanceWithDetailsAsync()
        {
        var maintanences = await _maintanenceRepo.GetAllMaintenanceWithDetailsAsync();
            return _mapper.Map<List<MaintenanceDetailDto>>(maintanences);
        }

        public async Task<MaintenanceDetailDto> AddMaintanenceAsync(CreateMaintenanceDto createMaintenanceDto)
        {
            var maintenance = _mapper.Map<Maintenance>(createMaintenanceDto);

            var validation= await _validator.ValidateAsync(maintenance);
            if (!validation.IsValid)
            {
                throw new ValidationException(validation.Errors);
            }
            if (createMaintenanceDto.ProductIds?.Any() == true)
            {
                var existingProductIds = _productRepository.Where(p => createMaintenanceDto.ProductIds.Contains(p.Id)).Select(p => p.Id).ToList();

                var missingProductIds = createMaintenanceDto.ProductIds?.Except(existingProductIds).ToList();

                if(missingProductIds?.Any() == true)
                
                    throw new ValidationException($"Geçersiz Ürün Id(ler)i: {string.Join(", ", missingProductIds)}");
                
                maintenance.MaintenanceProducts = existingProductIds.Select(id => new MaintenanceProduct
                {
                    ProductId = id,
                    Maintenance = maintenance
                }).ToList();

            }
            await _maintanenceRepo.AddAsync(maintenance);
            await _unitOfWork.SaveChangesAsync();
            return _mapper.Map<MaintenanceDetailDto>(maintenance);
        }


        public async Task<MaintenanceDetailDto?> GetByIdAsync(int id)
        {
           var maintenance =  await _maintanenceRepo.GetMaintenanceWithDetailsAsync(id);
            if(maintenance == null)
            
                return null;
            
          return _mapper.Map<MaintenanceDetailDto>(maintenance);
        }

        public async Task UpdateMaintenanceDto(UpdateMaintenanceDto dto, string updatedByUserName)
        {
            // Bakımı getir (detaylı)
            var maintenance = await _maintanenceRepo.GetMaintenanceWithDetailsAsync(dto.Id)
                ?? throw new KeyNotFoundException($"ID'si {dto.Id} olan bakım kaydı bulunamadı.");

            // Eski değerleri al
            var oldValues = _mapper.Map<MaintenanceDetailDto>(maintenance);

            // DTO ile mevcut entity'yi güncelle
            _mapper.Map(dto, maintenance);

            // Validation
            var validation = await _validator.ValidateAsync(maintenance);
            if (!validation.IsValid)
                throw new ValidationException(validation.Errors);

            // Ürünleri güncelle
            maintenance.MaintenanceProducts.Clear();
            if (dto.ProductIds?.Any() == true)
            {
                var existingProductIds = _productRepository
                    .Where(p => dto.ProductIds.Contains(p.Id))
                    .Select(p => p.Id)
                    .ToList();

                foreach (var pid in existingProductIds)
                {
                    maintenance.MaintenanceProducts.Add(new MaintenanceProduct
                    {
                        MaintenanceId = maintenance.Id,
                        ProductId = pid
                    });
                }
            }

            // Repository üzerinden güncelle
            _maintanenceRepo.Update(maintenance);
            await _unitOfWork.SaveChangesAsync();

            // Mail Gönderimi
            if (maintenance.Customer != null) // Customer null olabilir, kontrol edelim
            {
                var subject = $"{maintenance.Customer.CompanyName} bakım anlaşması güncellendi";
                var body = $@"
                    <p>{maintenance.Customer.CompanyName} firmasının bakım anlaşması {updatedByUserName} tarafından güncellenmiştir.</p>

                    <h4>Eski Anlaşma Bilgileri:</h4>
                    <ul>
                      <li>Başlangıç Tarihi: {oldValues.StartDate:yyyy-MM-dd}</li>
                      <li>Bitiş Tarihi: {oldValues.EndDate:yyyy-MM-dd}</li>
                      <li>Açıklama: {oldValues.Description}</li>
                      <li>Teklif Durumu: {oldValues.OfferStatus}</li>
                      <li>Anlaşma Durumu: {oldValues.ContractStatus}</li>
                      <li>Lisans Durumu: {oldValues.LicenseStatus}</li>
                      <li>Firma Durumu: {oldValues.FirmSituation}</li>
                    </ul>

                    <h4>Yeni Anlaşma Bilgileri:</h4>
                    <ul>
                      <li>Başlangıç Tarihi: {maintenance.StartDate:yyyy-MM-dd}</li>
                      <li>Bitiş Tarihi: {maintenance.EndDate:yyyy-MM-dd}</li>
                      <li>Açıklama: {maintenance.Description}</li>
                      <li>Teklif Durumu: {EnumHelper.GetDisplayName(maintenance.OfferStatus)}</li>
                      <li>Anlaşma Durumu: {EnumHelper.GetDisplayName(maintenance.ContractStatus)}</li>
                      <li>Lisans Durumu: {EnumHelper.GetDisplayName(maintenance.LicenseStatus)}</li>
                      <li>Firma Durumu: {EnumHelper.GetDisplayName(maintenance.FirmSituation)}</li>
                    </ul>";

                await _emailService.SendEmailAsync("ciftciserhatapp@gmail.com", subject, body);
            }
        }
        public async Task DeleteMaintenanceAsync(int id)
        {
            var maintenance =await _maintanenceRepo.GetByIdAsync(id);
            if( maintenance != null)
            {
                _maintanenceRepo.Delete(maintenance);
                await _unitOfWork.SaveChangesAsync();
            }

        }

    }
}
