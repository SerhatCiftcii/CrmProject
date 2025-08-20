using AutoMapper;
using CrmProject.Application.DTOs.CustomerChangeLogDtos;
using CrmProject.Application.Interfaces;
using CrmProject.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CrmProject.Application.Services.CustomerChangeLogServices
{
    public class CustomerChangeLogService : ICustomerChangeLogService
    {
        private readonly IGenericRepository<CustomerChangeLog> _changeLogRepo;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public CustomerChangeLogService(
            IGenericRepository<CustomerChangeLog> changeLogRepo,
            IUnitOfWork unitOfWork,
            IMapper mapper)
        {
            _changeLogRepo = changeLogRepo;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        // Belirli bir müşterinin logları
        public async Task<IEnumerable<CustomerChangeLogDto>> GetLogsByCustomerIdAsync(int customerId)
        {
            var logs = _changeLogRepo
                .Where(c => c.CustomerId == customerId)
                .Include(c => c.Customer)
                .Include(c => c.ChangedByUser)
                .OrderByDescending(c => c.ChangedAt)
                .ToList();

            return _mapper.Map<IEnumerable<CustomerChangeLogDto>>(logs);
        }

        // Tüm loglar
        public async Task<IEnumerable<CustomerChangeLogDto>> GetAllLogsAsync()
        {
            var logs = _changeLogRepo
                .GetAll()
                .Include(c => c.Customer)
                .Include(c => c.ChangedByUser)
                .OrderByDescending(c => c.ChangedAt)
                .ToList();

            return _mapper.Map<IEnumerable<CustomerChangeLogDto>>(logs);
        }

        // Yeni log ekleme
        public async Task AddChangeLogAsync(CreateCustomerChangeLogDto dto)
        {
            var log = _mapper.Map<CustomerChangeLog>(dto);
            log.ChangedAt = DateTime.UtcNow;

            await _changeLogRepo.AddAsync(log);
            await _unitOfWork.SaveChangesAsync();
        }
    }
}
