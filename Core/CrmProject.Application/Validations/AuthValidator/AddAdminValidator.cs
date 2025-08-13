using CrmProject.Application.Dtos.AuthDtos;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrmProject.Application.Validations.AuthValidator
{
    public class AddAdminValidator : AbstractValidator<AddAdminDto>
    {
        public AddAdminValidator()
        {
            RuleFor(x => x.Username)
                .NotEmpty().WithMessage("Username zorunludur.");

            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email zorunludur.")
                .EmailAddress().WithMessage("Geçerli bir email giriniz.");

            RuleFor(x => x.FullName)
                .NotEmpty().WithMessage("FullName zorunludur.");

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Password zorunludur.")
                .MinimumLength(6).WithMessage("Password minimum 6 karakter olmalıdır.");

            RuleFor(x => x.PhoneNumber)
                .Matches(@"^\+?\d*$").WithMessage("PhoneNumber geçerli formatta olmalıdır.")
                .When(x => !string.IsNullOrEmpty(x.PhoneNumber));
        }
    }
}
