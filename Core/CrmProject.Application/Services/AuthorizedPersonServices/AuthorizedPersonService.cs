using AutoMapper;
using CrmProject.Application.Dtos.AuthorizedPersonDtos;
using CrmProject.Application.Interfaces;
using CrmProject.Application.Validations;
using CrmProject.Domain.Entities;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace CrmProject.Application.Services.AuthorizedPersonServices
{
    public class AuthorizedPersonService : IAuthorizedPersonService
    {
        private readonly IAuthorizedPersonRepository _authorizedPersonRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly AuthorizedPersonValidator _validator;

        public AuthorizedPersonService(
            IAuthorizedPersonRepository authorizedPersonRepository,
            IUnitOfWork unitOfWork,
            IMapper mapper,
            AuthorizedPersonValidator validator)
        {
            _authorizedPersonRepository = authorizedPersonRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _validator = validator;
        }

        public async Task<List<AuthorizedPersonDto>> GetAllAuthorizedPersonAsync()
        {
            var list = await _authorizedPersonRepository.GetAll()
                .Include(ap => ap.Customer)
                .ToListAsync();

            return _mapper.Map<List<AuthorizedPersonDto>>(list);
        }

        public async Task<AuthorizedPersonDto?> GetAuthorizedPersonByIdAsync(int id)
        {
            var entity = await _authorizedPersonRepository.GetAll()
                .Include(ap => ap.Customer)
                .FirstOrDefaultAsync(ap => ap.Id == id);

            if (entity == null) return null;
            return _mapper.Map<AuthorizedPersonDto>(entity);
        }

        public async Task<AuthorizedPersonDto> AddAuthorizedPersonAsync(CreateAuthorizedPersonDto dto)
        {
            var entity = _mapper.Map<AuthorizedPerson>(dto);

            var validationResult = await _validator.ValidateAsync(entity);
            if (!validationResult.IsValid)
                throw new ValidationException(validationResult.Errors);

            await _authorizedPersonRepository.AddAsync(entity);
            await _unitOfWork.SaveChangesAsync();

            return _mapper.Map<AuthorizedPersonDto>(entity);
        }

        public async Task UpdateAuthorizedPersonAsync(UpdateAuthorizedPersonDto dto)
        {
            var entity = await _authorizedPersonRepository.GetByIdAsync(dto.Id);
            if (entity == null) throw new KeyNotFoundException($"ID {dto.Id} bulunamadı.");

            _mapper.Map(dto, entity);

            var validationResult = await _validator.ValidateAsync(entity);
            if (!validationResult.IsValid)
                throw new ValidationException(validationResult.Errors);

            _authorizedPersonRepository.Update(entity);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task DeleteAuthorizedPersonAsync(int id)
        {
            var entity = await _authorizedPersonRepository.GetByIdAsync(id);
            if (entity != null)
            {
                _authorizedPersonRepository.Delete(entity);
                await _unitOfWork.SaveChangesAsync();
            }
        }
    }
}
