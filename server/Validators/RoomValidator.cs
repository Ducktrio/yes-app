using System;
using FluentValidation;
using Yes.Contracts;
using Yes.Repositories;

namespace Yes.Validators;

public class CreateRoomValidator : AbstractValidator<CreateRoomContract>
{
    public CreateRoomValidator(IRoomTypeRepository roomTypeRepository)
    {
        RuleFor(x => x.RoomType_id)
            .NotEmpty().WithMessage("Room Type ID is required.")
            .MustAsync(async (roomTypeId, cancellation) =>
                !string.IsNullOrEmpty(roomTypeId) && await roomTypeRepository.Exists(roomTypeId)
            ).WithMessage("Room Type ID does not exist.");

        RuleFor(x => x.Label)
            .NotEmpty().WithMessage("Label is required.")
            .Length(3, 100).WithMessage("Label must be between 3 and 100 characters long.");
    }
}

public class UpdateRoomValidator : AbstractValidator<UpdateRoomContract>
{
    public UpdateRoomValidator(IRoomTypeRepository roomTypeRepository)
    {
        When(x => !string.IsNullOrEmpty(x.RoomType_id), () =>
        {
            RuleFor(x => x.RoomType_id)
                .NotEmpty().WithMessage("Room Type ID is required.")
                .MustAsync(async (roomTypeId, cancellation) =>
                    !string.IsNullOrEmpty(roomTypeId) && await roomTypeRepository.Exists(roomTypeId)
                ).WithMessage("Room Type ID does not exist.");
        });
        When(x => !string.IsNullOrEmpty(x.Label), () =>
        {
            RuleFor(x => x.Label)
                .NotEmpty().WithMessage("Label is required.")
                .Length(3, 100).WithMessage("Label must be between 3 and 100 characters long.");
        });
        When(x => x.Status.HasValue, () =>
        {
            RuleFor(x => x.Status)
                .NotEmpty().WithMessage("Status is required.")
                .InclusiveBetween(0, 2).WithMessage("Status must be between 0 and 2.");
        });
    }
}