using System;
using FluentValidation;
using Yes.Contracts;
using Yes.Repositories;

namespace Yes.Validators;

public class CreateUserValidator : AbstractValidator<CreateUserContract>
{
    public CreateUserValidator(IRoleRepository roleRepository)
    {
        RuleFor(x => x.Username)
            .NotEmpty().WithMessage("Username is required.")
            .Length(3, 100).WithMessage("Username must be between 3 and 100 characters long.");

        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("Password is required.")
            .Length(8, 100).WithMessage("Password must be between 8 and 100 characters long.")
            .Matches(@"[A-Z]").WithMessage("Password must contain at least one uppercase letter.")
            .Matches(@"[a-z]").WithMessage("Password must contain at least one lowercase letter.")
            .Matches(@"[0-9]").WithMessage("Password must contain at least one number.");

        RuleFor(x => x.Role_id)
            .NotEmpty().WithMessage("Role ID is required.")
            .MustAsync(async (roleId, cancellation) =>
                !string.IsNullOrEmpty(roleId) && await roleRepository.Exists(roleId)
            ).WithMessage("Role ID does not exist."); ;

        RuleFor(x => x.Description)
            .NotEmpty().WithMessage("Description is required.")
            .MaximumLength(255).WithMessage("Description must not exceed 255 characters.");
    }
}

public class UpdateUserValidator : AbstractValidator<UpdateUserContract>
{
    public UpdateUserValidator(IRoleRepository roleRepository)
    {
        When(x => !string.IsNullOrEmpty(x.Username), () =>
        {
            RuleFor(x => x.Username)
                .NotEmpty().WithMessage("Username is required.")
                .Length(3, 100).WithMessage("Username must be between 3 and 100 characters long.");
        });
        When(x => !string.IsNullOrEmpty(x.Password), () =>
        {
            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Password is required.")
                .Length(8, 100).WithMessage("Password must be between 8 and 100 characters long.")
                .Matches(@"[A-Z]").WithMessage("Password must contain at least one uppercase letter.")
                .Matches(@"[a-z]").WithMessage("Password must contain at least one lowercase letter.")
                .Matches(@"[0-9]").WithMessage("Password must contain at least one number.");
        });
        When(x => !string.IsNullOrEmpty(x.Role_id), () =>
        {
            RuleFor(x => x.Role_id)
                .NotEmpty().WithMessage("Role ID is required.")
                .MustAsync(async (roleId, cancellation) =>
                    !string.IsNullOrEmpty(roleId) && await roleRepository.Exists(roleId)
                ).WithMessage("Role ID does not exist.");
        });
        When(x => !string.IsNullOrEmpty(x.Description), () =>
        {
            RuleFor(x => x.Description)
                .NotEmpty().WithMessage("Description is required.")
                .MaximumLength(255).WithMessage("Description must not exceed 255 characters.");
        });
    }
}