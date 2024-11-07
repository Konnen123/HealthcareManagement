using Application.Use_Cases.Commands.LocationCommands;
using FluentValidation;

namespace Application.Validators.LocationValidators;

public class CreateLocationValidator : AbstractValidator<CreateLocationCommand>
{
    private const int MaxRoomNo = 250;
    private const int MaxFloorNo = 4;
    private const int MaxIndicationsLength = 200;
    public CreateLocationValidator()
    {
        RuleFor(command => command.RoomNo)
            .NotEmpty().WithMessage("Room number is required.")
            .InclusiveBetween(0, MaxRoomNo).WithMessage($"Room number must be between 0 and {MaxRoomNo}.");
        
        RuleFor(command => command.Floor)
            .InclusiveBetween(0, MaxFloorNo).WithMessage($"Floor must be between 0 and {MaxFloorNo}.");
        
        RuleFor(command => command.Indications)
            .MaximumLength(MaxIndicationsLength).WithMessage($"Indications cannot exceed {MaxIndicationsLength} characters.");
    }
}