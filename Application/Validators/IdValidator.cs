using Application.Use_Cases.Commands;
using Application.Use_Cases.Commands.AppointmentCommands;
using Application.Utils;
using FluentValidation;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace Application.Validators
{
    public class IdValidator<T> : AbstractValidator<T> where T : IdCommand
    {
        public IdValidator()
        {
            RuleFor(x => x.Id)
               .NotEmpty().WithMessage("ID is required.")
               .Must(GuidChecker.BeAValidGuid).WithMessage("ID must be a valid GUID")
               .NotEqual(Guid.Empty).WithMessage("ID cannot be an empty GUID.");
        }

    }
}
