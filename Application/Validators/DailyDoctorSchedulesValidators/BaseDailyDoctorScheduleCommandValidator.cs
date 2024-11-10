using Application.Use_Cases.Commands.SchedulesCommands;
using Application.Utils;
using FluentValidation;

namespace Application.Validators.DailyDoctorSchedulesValidators
{
    public class BaseDailyDoctorScheduleCommandValidator<T> : AbstractValidator<T> where T : BaseDailyDoctorScheduleCommand
    {
        public BaseDailyDoctorScheduleCommandValidator()
        {
            RuleFor(command => command.DayOfWeek)
                .IsInEnum()
                .WithMessage("DayOfWeek must be a valid day of the week.");


            RuleFor(command => command.DoctorId)
                .NotEmpty().WithMessage("Doctor ID is required.")
                .Must(GuidChecker.BeAValidGuid).WithMessage("Doctor ID must be a valid GUID")
                .NotEqual(Guid.Empty).WithMessage("Doctor ID cannot be an empty GUID.");

           
            RuleFor(command => command.LocationId)
                .NotEmpty().WithMessage("Location ID is required.")
                .Must(GuidChecker.BeAValidGuid).WithMessage("Location ID must be a valid GUID")
                .NotEqual(Guid.Empty).WithMessage("Location ID cannot be an empty GUID.");

            RuleFor(command => command.StartingTime)
                .NotEmpty()
                .WithMessage("Starting time cannot be empty.");

            RuleFor(command => command.EndingTime)
                .NotEmpty()
                .WithMessage("Ending time cannot be empty.");

            RuleFor(command => command.EndingTime)
                .GreaterThan(command => command.StartingTime)
                .WithMessage("Ending time must be later than starting time.");

            RuleFor(command => command.SlotDurationMinutes)
                .GreaterThan(0)
                .WithMessage("SlotDurationMinutes must be a positive number.");
        }
    }
}
