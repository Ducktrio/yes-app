using System;
using FluentValidation;
using Yes.Contracts;

namespace Yes.Validators;

public class CreateCustomerValidator : AbstractValidator<CreateCustomerContract>
{
    public CreateCustomerValidator()
    {
        RuleFor(x => x.Courtesy_title)
            .NotEmpty().WithMessage("Courtesy title is required.")
            .MaximumLength(100).WithMessage("Courtesy title must not exceed 100 characters.");

        RuleFor(x => x.Full_name)
            .NotEmpty().WithMessage("Full name is required.")
            .Length(3, 100).WithMessage("Full name must be between 3 and 100 characters long.");

        RuleFor(x => x.Age)
            .NotEmpty().WithMessage("Age is required.")
            .GreaterThan(0).WithMessage("Age must be greater than 0.");

        RuleFor(x => x.Phone_number)
            .MaximumLength(100).WithMessage("Phone number must not exceed 100 characters.");
            
        RuleFor(x => x.Contact_info)
            .MaximumLength(255).WithMessage("Contact info must not exceed 255 characters.");
    }
}

public class UpdateCustomerValidator : AbstractValidator<UpdateCustomerContract>
{
    public UpdateCustomerValidator()
    {
        When(x => !string.IsNullOrEmpty(x.Courtesy_title), () =>
        {
            RuleFor(x => x.Courtesy_title)
                .NotEmpty().WithMessage("Courtesy title is required.")
                .MaximumLength(100).WithMessage("Courtesy title must not exceed 100 characters.");
        });
        When(x => !string.IsNullOrEmpty(x.Full_name), () =>
        {
            RuleFor(x => x.Full_name)
                .NotEmpty().WithMessage("Full name is required.")
                .Length(3, 100).WithMessage("Full name must be between 3 and 100 characters long.");
        });
        When(x => x.Age > 0, () =>
        {
            RuleFor(x => x.Age)
                .NotEmpty().WithMessage("Age is required.")
                .GreaterThan(0).WithMessage("Age must be greater than 0.");
        });
        When(x => !string.IsNullOrEmpty(x.Phone_number), () =>
        {
            RuleFor(x => x.Phone_number)
                .MaximumLength(100).WithMessage("Phone number must not exceed 100 characters.");
        });
        When(x => !string.IsNullOrEmpty(x.Contact_info), () =>
        {
            RuleFor(x => x.Contact_info)
                .MaximumLength(255).WithMessage("Contact info must not exceed 255 characters.");
        });
    }
}