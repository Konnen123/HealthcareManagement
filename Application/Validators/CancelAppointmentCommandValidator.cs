using Application.Use_Cases.Commands;
using Application.Utils;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Validators
{
    public class CancelAppointmentCommandValidator :AbstractValidator<CancelAppointmentCommand>
    {
        public CancelAppointmentCommandValidator()
        {
            RuleFor(x => x.PatientId)
                .NotEmpty().WithMessage("Patient ID is required.")
                .Must(GuidChecker.BeAValidGuid).WithMessage("Patiend ID must be a valid GUID")
                .NotEqual(Guid.Empty).WithMessage("Patient ID cannot be an empty GUID.");

            RuleFor(x => x.AppointmentId)
                .NotEmpty().WithMessage("Appointment ID is required.")
                .Must(GuidChecker.BeAValidGuid).WithMessage("Appointment ID must be a valid GUID")
                .NotEqual(Guid.Empty).WithMessage("Appointment ID cannot be an empty GUID.");
        }
    }
}
