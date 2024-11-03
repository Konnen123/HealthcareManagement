using Application.Use_Cases.Commands.AppointmentCommands;
using FluentValidation;

namespace Application.Validators;

public class RescheduleAppointmentValidator : AbstractValidator<RescheduleAppointmentCommand>
{
    public RescheduleAppointmentValidator()
    {
        RuleFor(command => command)
            .Must(cmd => cmd.NewDate.HasValue || cmd.NewStartTime.HasValue)
            .WithMessage("At least one attribute must change.");
    }
}