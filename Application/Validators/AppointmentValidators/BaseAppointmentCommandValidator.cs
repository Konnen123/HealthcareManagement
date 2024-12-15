using Application.Use_Cases.Commands;
using Application.Utils;
using FluentValidation;

//Ar trebui regandita arhitectura la validatori,TO DO.
namespace Application.Validators.AppointmentValidators
{
    public class BaseAppointmentCommandValidator<T> : AbstractValidator<T> where T : BaseAppointmentCommand
    {
        public BaseAppointmentCommandValidator()
        {
            RuleFor(x => x.DoctorId)
                .NotEmpty().WithMessage("Doctor ID is required.")
                .Must(GuidChecker.BeAValidGuid).WithMessage("Doctor ID must be a valid GUID")
                .NotEqual(Guid.Empty).WithMessage("Doctor ID cannot be an empty GUID.");

            RuleFor(x => x.Date)
                .NotEmpty().WithMessage("Date is required.")
                .Must(BeAValidDate).WithMessage("Date must be a valid date.")
                .GreaterThanOrEqualTo(DateOnly.FromDateTime(DateTime.Today)).WithMessage("Date cannot be in the past.");

            RuleFor(x => x.StartTime)
                .NotEmpty().WithMessage("Start time is required.");

            RuleFor(x => x.EndTime)
                .NotEmpty().WithMessage("End time is required.")
                .GreaterThan(x => x.StartTime).WithMessage("End time must be after the start time.");
            
            RuleFor(x => x.EndTime)
                .NotEmpty().WithMessage("End time is required.")
                .GreaterThan(x => x.StartTime).WithMessage("End time must be after the start time.");
            
            RuleFor(x => x.UserNotes)
                .MaximumLength(500).WithMessage("User notes cannot exceed 500 characters.");
        }

        private static bool BeAValidDate(DateOnly date)
        {
            return date != default;
        }
    }
}