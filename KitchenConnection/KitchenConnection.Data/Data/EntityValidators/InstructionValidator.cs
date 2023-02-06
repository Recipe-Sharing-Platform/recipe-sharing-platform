using FluentValidation;
using KitchenConnection.Models.DTOs.Instruction;

namespace KitchenConnection.DataLayer.Data.EntityValidators {
    public class InstructionValidator : AbstractValidator<RecipeInstructionCreateDTO>
    {
        public InstructionValidator()
        {
            RuleFor(x => x.StepDescription)
                .Cascade(CascadeMode.StopOnFirstFailure)
                .NotNull().NotEmpty()
                    .WithMessage("Step description is required!")
                .MinimumLength(150)
                    .WithMessage("Step description must have 150 or more characters!");
        }
    }
}
