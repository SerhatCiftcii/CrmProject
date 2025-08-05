using AutoMapper;
using CrmProject.Application.Dtos.MaintenanceDtos;
using CrmProject.Application.Interfaces;
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

        private readonly MaintenanceValidator _validator;

        public MaintenanceService(IMaintenanceRepository maintanenceRepo, IUnitOfWork unitOfWork, IMapper mapper, MaintenanceValidator validator, IGenericRepository<Product> productRepository)
        {
            _maintanenceRepo = maintanenceRepo;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _validator = validator;
            _productRepository = productRepository;
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

        public async Task UpdateMaintenanceDto(UpdateMaintenanceDto dto)
        {
            var maintenance = await _maintanenceRepo.GetMaintenanceWithDetailsAsync(dto.Id) ?? throw new KeyNotFoundException($"ID'si  {dto.Id} olan bakım kaydı bulunamadı.");
            _mapper.Map(dto, maintenance);

            var validation = await _validator.ValidateAsync(maintenance);
            if (!validation.IsValid)
            
                throw new ValidationException(validation.Errors);

            //Mevcut ürünnleri temizle ve yeni ürünleri ekle
            maintenance.MaintenanceProducts.Clear();
            if (dto.ProductIds?.Any() == true)
            {
                var existingProductIds = _productRepository.Where(p => dto.ProductIds.Contains(p.Id))
                                                          .Select(p => p.Id).ToList();

                foreach (var pid in existingProductIds)
                {
                    maintenance.MaintenanceProducts.Add(new MaintenanceProduct
                    {
                        MaintenanceId = maintenance.Id,
                        ProductId = pid
                    });
                }
            }

            _maintanenceRepo.Update(maintenance);
            await _unitOfWork.SaveChangesAsync();


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
