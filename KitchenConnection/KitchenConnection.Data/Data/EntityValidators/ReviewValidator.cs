using FluentValidation;
using KitchenConnection.DataLayer.Models.DTOs.Review;
using KitchenConnection.DataLayer.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KitchenConnection.DataLayer.Data.EntityValidators
{
    public class ReviewValidator : AbstractValidator<ReviewCreateDTO>
    {
        public ReviewValidator()
        {
            RuleFor(x => x.Rating)
                .Cascade(CascadeMode.Stop)
                .NotNull().NotEmpty()
                    .WithMessage("Rating is required!")
                .GreaterThanOrEqualTo(0)
                .LessThanOrEqualTo(10)
                    .WithMessage("Rating can be between 1 and 5 stars!");

            RuleFor(x => x.Message)
                .Cascade(CascadeMode.Stop)
                .MinimumLength(10)
                    .WithMessage("Review Message must have 10 or more characters!");
        }
    }
}
