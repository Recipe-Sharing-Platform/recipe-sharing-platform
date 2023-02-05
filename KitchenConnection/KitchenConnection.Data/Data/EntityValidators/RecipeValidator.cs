using FluentValidation;
using KitchenConnection.DataLayer.Models.DTOs.Recipe;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace KitchenConnection.DataLayer.Data.EntityValidators
{
    public class RecipeValidator : AbstractValidator<RecipeCreateDTO>
    {
        public RecipeValidator()
        {
            RuleFor(x => x.Name)
                .NotNull().NotEmpty()
                    .WithMessage("Recipe Name is required!")
                .Length(5, 50)
                    .WithMessage("Recipe Name must have between 5 and 50 characters");

            RuleFor(x => x.Description)
                .MinimumLength(50)
                    .WithMessage("Description must have 50 or more characters!");

            RuleFor(x => x.VideoInstructions)
                .Matches(new Regex("^https?:\\/\\/(?:www\\.)?[-a-zA-Z0-9@:%._\\+~#=]{1,256}\\.[a-zA-Z0-9()]{1,6}\\b(?:[-a-zA-Z0-9()@:%_\\+.~#?&\\/=]*)$"))
                    .WithMessage("Video Instructions must be a valid url!");

            RuleFor(x => x.AudioInstructions)
                .Matches(new Regex("^https?:\\/\\/(?:www\\.)?[-a-zA-Z0-9@:%._\\+~#=]{1,256}\\.[a-zA-Z0-9()]{1,6}\\b(?:[-a-zA-Z0-9()@:%_\\+.~#?&\\/=]*)$"))
                    .WithMessage("Audio Instructions must be a valid url!");

            RuleForEach(x => x.Ingredients).SetValidator(new IngredientValidator());

            RuleForEach(x => x.Instructions).SetValidator(new InstructionValidator());

            RuleForEach(x => x.Tags).SetValidator(new TagValidator());
        }
    }
}
