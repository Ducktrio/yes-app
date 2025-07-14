using System;
using FluentValidation;
using Yes.Contracts;
using Yes.Repositories;

namespace Yes.Validators;

public class CreateRoomTicketValidator : AbstractValidator<CreateRoomTicketContract>
{
    public CreateRoomTicketValidator(ICustomerRepository customerRepository, IRoomRepository roomRepository)
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

        RuleFor(x => x.Number_of_occupants)
            .NotEmpty().WithMessage("Number of occupants is required.")
            .GreaterThan(0).WithMessage("Number of occupants must be greater than 0.");
    }
}

public class UpdateRoomTicketValidator : AbstractValidator<UpdateRoomTicketContract>
{
    public UpdateRoomTicketValidator(ICustomerRepository customerRepository, IRoomRepository roomRepository)
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
        When(x => x.Number_of_occupants.HasValue, () =>
        {
            RuleFor(x => x.Number_of_occupants)
                .NotEmpty().WithMessage("Number of occupants is required.")
                .GreaterThan(0).WithMessage("Number of occupants must be greater than 0.");
        });
    }
}