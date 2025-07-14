using System;
using FluentValidation;
using Yes.Contracts;

namespace Yes.Validators;

public class CreateServiceValidator : AbstractValidator<CreateServiceContract>
{
    public CreateServiceValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Name is required.")
            .Length(3, 100).WithMessage("Name must be between 3 and 100 characters long.");

        RuleFor(x => x.Price)
            .NotEmpty().WithMessage("Price is required.")
            .GreaterThanOrEqualTo(0).WithMessage("Price must be a non-negative integer.");
    }
}

public class UpdateServiceValidator : AbstractValidator<UpdateServiceContract>
{
    public UpdateServiceValidator()
    {
        When(x => !string.IsNullOrEmpty(x.Name), () =>
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Name is required.")
                .Length(3, 100).WithMessage("Name must be between 3 and 100 characters long.");
        });
        When(x => x.Price.HasValue, () =>
        {
            RuleFor(x => x.Price)
                .NotEmpty().WithMessage("Price is required.")
                .GreaterThanOrEqualTo(0).WithMessage("Price must be a non-negative integer.");
        });
    }
}