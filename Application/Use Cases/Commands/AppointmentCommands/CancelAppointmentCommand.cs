using Application.Use_Cases.Commands.AppointmentCommands;
using Domain.Utils;
using MediatR;

namespace Application.Use_Cases.Commands;

public class CancelAppointmentCommand : CancelIdCommand, IRequest<Result<Unit>>
{
    public string CancellationReason { get; set; }
}