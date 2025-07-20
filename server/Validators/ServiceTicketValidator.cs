using System;
using FluentValidation;
using Yes.Contracts;
using Yes.Repositories;

namespace Yes.Validators;

public class CreateServiceTicketValidator : AbstractValidator<CreateServiceTicketContract>
{
    public CreateServiceTicketValidator(ICustomerRepository customerRepository, IRoomRepository roomRepository, IServiceRepository serviceRepository)
    {
        RuleFor(x => x.Customer_id)
            .NotEmpty().WithMessage("Customer ID is required.")
            .MustAsync(async (customerId, cancellation) =>
                !string.IsNullOrEmpty(customerId) && await customerRepository.Exists(customerId)
            ).WithMessage("Customer ID does not exist.");

        RuleFor(x => x.Room_id)
            .NotEmpty().WithMessage("Room ID is required.")
            .MustAsync(async (roomId, cancellation) =>
                !string.IsNullOrEmpty(roomId) && await roomRepository.Exists(roomId)
            ).WithMessage("Room ID does not exist.");

        RuleFor(x => x.Service_id)
            .NotEmpty().WithMessage("Service ID is required.")
            .MustAsync(async (serviceId, cancellation) =>
                !string.IsNullOrEmpty(serviceId) && await serviceRepository.Exists(serviceId)
            ).WithMessage("Service ID does not exist.");

        RuleFor(x => x.Details)
            .NotEmpty().WithMessage("Details are required.")
            .MaximumLength(500).WithMessage("Details must not exceed 500 characters.");
    }
}

public class UpdateServiceTicketValidator : AbstractValidator<UpdateServiceTicketContract>
{
    public UpdateServiceTicketValidator(ICustomerRepository customerRepository, IRoomRepository roomRepository, IServiceRepository serviceRepository)
    {
        When(x => !string.IsNullOrEmpty(x.Customer_id), () =>
        {
            RuleFor(x => x.Customer_id)
                .NotEmpty().WithMessage("Customer ID is required.")
                .MustAsync(async (customerId, cancellation) =>
                    !string.IsNullOrEmpty(customerId) && await customerRepository.Exists(customerId)
                ).WithMessage("Customer ID does not exist.");
        });

        When(x => !string.IsNullOrEmpty(x.Room_id), () =>
        {
            RuleFor(x => x.Room_id)
                .NotEmpty().WithMessage("Room ID is required.")
                .MustAsync(async (roomId, cancellation) =>
                    !string.IsNullOrEmpty(roomId) && await roomRepository.Exists(roomId)
                ).WithMessage("Room ID does not exist.");
        });

        When(x => !string.IsNullOrEmpty(x.Service_id), () =>
        {
            RuleFor(x => x.Service_id)
                .NotEmpty().WithMessage("Service ID is required.")
                .MustAsync(async (serviceId, cancellation) =>
                    !string.IsNullOrEmpty(serviceId) && await serviceRepository.Exists(serviceId)
                ).WithMessage("Service ID does not exist.");
        });

        When(x => !string.IsNullOrEmpty(x.Details), () =>
        {
            RuleFor(x => x.Details)
                .NotEmpty().WithMessage("Details are required.")
                .MaximumLength(500).WithMessage("Details must not exceed 500 characters.");
        });
    }
}