using FluentValidation;
using KitchenConnection.DataLayer.Models.DTOs.CookBook;
using KitchenConnection.DataLayer.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KitchenConnection.DataLayer.Data.EntityValidators
{
    public class CookBookValidator : AbstractValidator<CookBookCreateDTO>
    {
        public CookBookValidator()
        {
            RuleFor(x => x.Name)
                .Cascade(CascadeMode.StopOnFirstFailure)
                .NotNull().NotEmpty()
                    .WithMessage("CookBook Name is required!")
                .MinimumLength(10)
                    .WithMessage("CookBook Name must have 10 or more characters!")
                .MaximumLength(30)
                    .WithMessage("CookBook Name can not have more than 30 characters!");

            RuleFor(x => x.Description)
                .Cascade(CascadeMode.StopOnFirstFailure)
                .MinimumLength(150)
                    .WithMessage("CookBook Description must have 150 or more characters!");
        }
    }
}
