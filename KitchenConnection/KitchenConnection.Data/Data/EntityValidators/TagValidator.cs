using FluentValidation;
using KitchenConnection.Models.DTOs.Recipe;
using KitchenConnection.Models.DTOs.Tag;
using KitchenConnection.Models.Entities;

namespace KitchenConnection.DataLayer.Data.EntityValidators;

public class TagValidator : AbstractValidator<TagCreateDTO>
{
    public TagValidator()
    {
        RuleFor(x => x.Name)
            .Cascade(CascadeMode.StopOnFirstFailure)
            .NotNull().NotEmpty()
                .WithMessage("Tag Name is required!")
            .MinimumLength(5)
                .WithMessage("Tag Name must have 5 or more characters")
            .MaximumLength(15)
                .WithMessage("Tag name can not have more than 15 characters");
    }
}
