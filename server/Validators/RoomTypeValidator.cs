using System;
using FluentValidation;
using Yes.Contracts;

namespace Yes.Validators;

public class CreateRoomTypeValidator : AbstractValidator<CreateRoomTypeContract>
{
    public CreateRoomTypeValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Name is required.")
            .Length(3, 100).WithMessage("Name must be between 3 and 100 characters long.");

        RuleFor(x => x.Description)
            .MaximumLength(255).WithMessage("Description must not exceed 255 characters.");

        RuleFor(x => x.Price)
            .NotEmpty().WithMessage("Price is required.")
            .GreaterThan(0).WithMessage("Price must be greater than 0.");
    }
}

public class UpdateRoomTypeValidator : AbstractValidator<UpdateRoomTypeContract>
{
    public UpdateRoomTypeValidator()
    {
        When(x => !string.IsNullOrEmpty(x.Name), () =>
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Name is required.")
                .Length(3, 100).WithMessage("Name must be between 3 and 100 characters long.");
        });
        When(x => !string.IsNullOrEmpty(x.Description), () =>
        {
            RuleFor(x => x.Description)
                .MaximumLength(255).WithMessage("Description must not exceed 255 characters.");
        });
        When(x => x.Price.HasValue, () =>
        {
            RuleFor(x => x.Price)
                .NotEmpty().WithMessage("Price is required.")
                .GreaterThan(0).WithMessage("Price must be greater than 0.");
        });
    }
}