using Application.Use_Cases.Commands.SchedulesCommands;
using Application.Utils;
using FluentValidation;

namespace Application.Validators.DailyDoctorSchedulesValidators
{
    public class UpdateDailyDoctorScheduleCommandValidator :BaseDailyDoctorScheduleCommandValidator<UpdateDailyDoctorScheduleCommand>
    {
        public UpdateDailyDoctorScheduleCommandValidator()
        {
            RuleFor(command => command.Id)
               .NotEmpty().WithMessage("Schedule ID is required.")
               .Must(GuidChecker.BeAValidGuid).WithMessage("Schedule ID must be a valid GUID")
               .NotEqual(Guid.Empty).WithMessage("Schedule ID cannot be an empty GUID.");

        }
    }
}
