using CrmProject.Domain.Entities;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrmProject.Application.Validations
{
    public class AuthorizedPersonValidator : AbstractValidator<AuthorizedPerson>
    {
        public AuthorizedPersonValidator()
        {
            RuleFor(x => x.Id).GreaterThan(0).WithMessage("ID geçerli olmalıdır.");

            RuleFor(x=>x.FullName).NotEmpty().WithMessage("Ad Soyad alanı boş bırakılamaz")
                .MaximumLength(100).WithMessage("Ad Soyad alanı en fazla 100 karakter olmalıdır.");

            RuleFor(x => x.Phone).NotEmpty().WithMessage("Telefon alanı boş bırakılamaz").
                MaximumLength(20).WithMessage("Telefon alanı en fazla 20 karakter olamalıdır");

            RuleFor(x => x.Email)
               .NotEmpty().WithMessage("Email alanı boş olamaz.")
               .EmailAddress().WithMessage("Geçerli bir email adresi girin.")
               .MaximumLength(100).WithMessage("Email en fazla 100 karakter olabilir.");

            // Unvan alanı için validasyon kuralları
            RuleFor(x => x.Title)
                .MaximumLength(50).WithMessage("Unvan en fazla 50 karakter olabilir.");

            // Notlar alanı için validasyon kuralları
            RuleFor(x => x.Notes)
                .MaximumLength(500).WithMessage("Notlar en fazla 500 karakter olabilir.");

            // Müşteri ID'si için validasyon kuralı
            RuleFor(x => x.CustomerId)
                .GreaterThan(0).WithMessage("Müşteri ID'si geçerli bir değer olmalıdır.");



        }
    }
}
