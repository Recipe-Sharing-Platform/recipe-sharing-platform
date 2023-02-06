using FluentValidation;
using KitchenConnection.Models.DTOs.Cuisine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KitchenConnection.DataLayer.Data.EntityValidators
{
    public class CuisineValidator : AbstractValidator<CuisineCreateDTO>
    {
        public CuisineValidator()
        {
            RuleFor(x => x.Name)
                .NotNull().NotEmpty()
                .WithMessage("Cuisine Name is required!")
                .Length(2, 15)
                .WithMessage("Cuisine Name must be between 2 and 15 characters!");
        }
    }
}
