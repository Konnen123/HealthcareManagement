using Domain.Utils;
using MediatR;

namespace Application.Use_Cases.Commands.AppointmentCommands
{
    public class DeleteAppointmentCommand : IRequest<Result<Unit>>
    {
        public Guid AppointmentId { get; set; }
    }
}
