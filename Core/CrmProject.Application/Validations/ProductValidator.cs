using CrmProject.Domain.Entities;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrmProject.Application.Validations
{
    public class ProductValidator : AbstractValidator<Product>
    {
        public ProductValidator()
        {
            RuleFor(x => x.Name)
                 .NotEmpty().WithMessage("Ürün adı boş bırakılamaz.")
                 .MaximumLength(250).WithMessage("Ürün adı en fazla 250 karakter olabilir.");

            RuleFor(x => x.Description)
                .MaximumLength(500).WithMessage("Ürün açıklaması en fazla 500 karakter olabilir.");

            RuleFor(x => x.Price)
                .NotEmpty().WithMessage("Ürün fiyatı boş bırakılamaz.")
                .GreaterThan(0).WithMessage("Ürün fiyatı sıfırdan büyük olmalıdır.");
        }

    }
}
