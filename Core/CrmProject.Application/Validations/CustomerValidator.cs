// CrmProject.Application/Validations/CustomerValidator.cs

using CrmProject.Domain.Entities; // Customer entity'si için
using FluentValidation;

namespace CrmProject.Application.Validations
{
    public class CustomerValidator : AbstractValidator<Customer>
    {
        public CustomerValidator()
        {
            RuleFor(x => x.CompanyName)
                .NotEmpty().WithMessage("Şirket adı boş bırakılamaz.")
                .MaximumLength(250).WithMessage("Şirket adı en fazla 250 karakter olabilir.");

            RuleFor(x => x.Email)
                .MaximumLength(100).WithMessage("Email adresi en fazla 100 karakter olabilir.")
                .EmailAddress().WithMessage("Geçerli bir email adresi giriniz.");

            RuleFor(x => x.Phone)
                .MaximumLength(50).WithMessage("Telefon numarası en fazla 50 karakter olabilir.");

            RuleFor(x => x.Address)
                .MaximumLength(500).WithMessage("Adres en fazla 500 karakter olabilir.");

            RuleFor(x => x.TaxNumber)
                .MaximumLength(50).WithMessage("Vergi numarası en fazla 50 karakter olabilir.");

            RuleFor(x => x.City)
                .MaximumLength(100).WithMessage("Şehir adı en fazla 100 karakter olabilir.");

            RuleFor(x => x.District)
                .MaximumLength(100).WithMessage("İlçe adı en fazla 100 karakter olabilir.");

            RuleFor(x => x.OwnerName)
                .MaximumLength(250).WithMessage("Sahip adı en fazla 250 karakter olabilir.");

            RuleFor(x => x.BranchName)
                .MaximumLength(250).WithMessage("Şube adı en fazla 250 karakter olabilir.");

            RuleFor(x => x.TaxOffice)
                .MaximumLength(100).WithMessage("Vergi dairesi en fazla 100 karakter olabilir.");

            RuleFor(x => x.WebSite)
                .MaximumLength(250).WithMessage("Web sitesi en fazla 250 karakter olabilir.");

            // IsActive alanı için de bir kural ekleyebiliriz, örneğin null olamaz gibi.
           /* RuleFor(x => x.IsActive)
                .NotNull().WithMessage("Aktiflik durumu belirtilmelidir.");*/
        }
    }
}
