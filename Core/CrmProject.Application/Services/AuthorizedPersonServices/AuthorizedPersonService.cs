using AutoMapper;
using CrmProject.Application.Dtos.AuthorizedPersonDtos;
using CrmProject.Application.Interfaces;
using CrmProject.Application.Validations;
using CrmProject.Domain.Entities;
using FluentValidation;
using Microsoft.AspNetCore.Identity;

namespace CrmProject.Application.Services.AuthorizedPersonServices
{
    public class AuthorizedPersonService : IAuthorizedPersonService
    {
        private readonly IAuthorizedPersonRepository _authorizedPersonRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<AppUser> _userManager;
        private readonly IMapper _mapper;
        private readonly AuthorizedPersonValidator _validator;

        public AuthorizedPersonService(
            IAuthorizedPersonRepository authorizedPersonRepository,
            IUnitOfWork unitOfWork,
            IMapper mapper,
            AuthorizedPersonValidator validator,
            UserManager<AppUser> userManager)
        {
            _authorizedPersonRepository = authorizedPersonRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _validator = validator;
            _userManager = userManager;
        }

        public async Task<List<AuthorizedPersonDto>> GetAllAuthorizedPersonAsync()
        {
            var authorizedPersons = _authorizedPersonRepository.GetAll().ToList();
            return _mapper.Map<List<AuthorizedPersonDto>>(authorizedPersons);
        }

        public async Task<AuthorizedPersonDto?> GetAuthorizedPersonByIdAsync(int id)
        {
            var authorizedPerson = await _authorizedPersonRepository.GetByIdAsync(id);
            if (authorizedPerson == null)
                return null;

            return _mapper.Map<AuthorizedPersonDto>(authorizedPerson);
        }

        public async Task<AuthorizedPersonDto> AddAuthorizedPersonAsync(CreateAuthorizedPersonDto dto)
        {
            var authorizedPerson = _mapper.Map<AuthorizedPerson>(dto);

            var validationResult = await _validator.ValidateAsync(authorizedPerson);
            if (!validationResult.IsValid)
                throw new ValidationException(validationResult.Errors);

            await _authorizedPersonRepository.AddAsync(authorizedPerson);
            await _unitOfWork.SaveChangesAsync();

            return _mapper.Map<AuthorizedPersonDto>(authorizedPerson);
        }

        public async Task UpdateAuthorizedPersonAsync(UpdateAuthorizedPersonDto dto)
        {
            var authorizedPerson = await _authorizedPersonRepository.GetByIdAsync(dto.Id);
            if (authorizedPerson == null)
                throw new KeyNotFoundException($"ID'si {dto.Id} olan yetkili kişi bulunamadı.");

            _mapper.Map(dto, authorizedPerson);

            var validationResult = await _validator.ValidateAsync(authorizedPerson);
            if (!validationResult.IsValid)
                throw new ValidationException(validationResult.Errors);

            _authorizedPersonRepository.Update(authorizedPerson);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task DeleteAuthorizedPersonAsync(int id)
        {
            var authorizedPerson = await _authorizedPersonRepository.GetByIdAsync(id);
            if (authorizedPerson != null)
            {
                _authorizedPersonRepository.Delete(authorizedPerson);
                await _unitOfWork.SaveChangesAsync();
            }
        }

        public async Task<string> ToggleStatusAsync(string currentUserId, ToggleAuthorizedPersonDto dto)
        {
            var currentUser = await _userManager.FindByIdAsync(currentUserId);
            if (currentUser == null || !currentUser.IsSuperAdmin)
                return "Bu işlemi yapmaya yetkiniz yok.";

            var person = await _authorizedPersonRepository.GetByIdAsync(dto.Id);
            if (person == null)
                return "Yetkili kişi bulunamadı.";

            person.IsActive = dto.IsActive;
            _authorizedPersonRepository.Update(person);
            await _unitOfWork.SaveChangesAsync();

            return "Durum başarıyla güncellendi.";
        }
    }
}
