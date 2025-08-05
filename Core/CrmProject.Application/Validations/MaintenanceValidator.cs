using CrmProject.Domain.Entities;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrmProject.Application.Validations
{
    public class MaintenanceValidator : AbstractValidator<Maintenance>
    {
        public MaintenanceValidator()
        {
            RuleFor(m => m.StartDate).LessThan(m => m.EndDate).WithMessage("Bitiş Tarihi, başlangıç tarihinde sonra olmalıdır.");

            RuleFor(m => m.EndDate).GreaterThan(m => m.StartDate).WithMessage("Bitiş tarihi , başlangıç tarihinden sonra olmalıdır.");

            When(m => m.PassportCreatedDate.HasValue, () =>
            {
                RuleFor(m => m.PassportCreatedDate.Value)
                    .LessThanOrEqualTo(DateTime.Now)
                    .WithMessage("Pasaport oluşturma tarihi gelecekte olamaz.");
            });

            RuleFor(m => m.Description)
                .MaximumLength(300)
                .WithMessage("Açıklama en fazla 300 karakter olabilir.");

            RuleFor(m => m.OfferStatus)
                .IsInEnum()
                .WithMessage("Geçerli bir teklif durumu seçilmelidir.");

            RuleFor(m => m.ContractStatus)
                .IsInEnum()
                .WithMessage("Geçerli bir sözleşme durumu seçilmelidir.");

            RuleFor(m => m.LicenseStatus)
                .IsInEnum()
                .WithMessage("Geçerli bir lisans durumu seçilmelidir.");

            RuleFor(m => m.FirmSituation)
                .IsInEnum()
                .WithMessage("Geçerli bir firma durumu seçilmelidir.");

        }
    }
}
