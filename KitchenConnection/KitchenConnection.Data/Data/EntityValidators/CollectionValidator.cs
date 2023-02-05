using FluentValidation;
using KitchenConnection.Models.DTOs.Collection;

namespace KitchenConnection.DataLayer.Data.EntityValidators {
    public class CollectionValidator : AbstractValidator<CollectionCreateDTO>
    {
        public CollectionValidator()
        {
            RuleFor(x => x.Name)
                .Cascade(CascadeMode.StopOnFirstFailure)
                .NotNull().NotEmpty()
                    .WithMessage("Collection Name is required!")
                .MinimumLength(10)
                    .WithMessage("Collection Name must have 10 or more characters!")
                .MaximumLength(30)
                    .WithMessage("Collection Name can not have more than 30 characters!");

            RuleFor(x => x.Description)
                .Cascade(CascadeMode.StopOnFirstFailure)
                .MinimumLength(150)
                    .WithMessage("Collection Description must have 150 or more characters!");
        }
    }
}
