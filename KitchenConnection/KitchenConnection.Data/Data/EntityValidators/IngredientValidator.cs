using FluentValidation;
using KitchenConnection.DataLayer.Models.DTOs.Recipe;
using KitchenConnection.DataLayer.Models.Entities.Mappings;

namespace KitchenConnection.DataLayer.Data.EntityValidators
{
    public class IngredientValidator : AbstractValidator<RecipeIngredientCreateDTO>
    {
        public IngredientValidator()
        {
            RuleFor(x => x.Name)
                .Cascade(CascadeMode.StopOnFirstFailure)
                .NotNull().NotEmpty()
                    .WithMessage("Ingredient Name is required!")
                .MinimumLength(3)
                    .WithMessage("Ingredient Name must have 3 or more characters!")
                .MaximumLength(15)
                    .WithMessage("Ingredient Name can not have more than 15 characters!");
        }
    }
}
