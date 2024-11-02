using Domain.Utils;
using MediatR;

namespace Application.Use_Cases.Commands.AppointmentCommands
{
    public class UpdateAppointmentCommand : BaseAppointmentCommand, IRequest<Result<Unit>>
    {
        public Guid Id { get; set; } 
    }
}
