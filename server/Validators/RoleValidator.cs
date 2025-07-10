using System;
using FluentValidation;
using Yes.Contracts;

namespace Yes.Validators;

public class CreateRoleValidator : AbstractValidator<CreateRoleContract>
{
    public CreateRoleValidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty().WithMessage("Title is required.")
            .Length(3, 100).WithMessage("Title must be between 3 and 100 characters long.");
    }
}

public class UpdateRoleValidator : AbstractValidator<UpdateRoleContract>
{
    public UpdateRoleValidator()
    {
        When(x => !string.IsNullOrEmpty(x.Title), () =>
        {
            RuleFor(x => x.Title)
                .NotEmpty().WithMessage("Title is required.")
                .Length(3, 100).WithMessage("Title must be between 3 and 100 characters long.");
        });
    }
}
