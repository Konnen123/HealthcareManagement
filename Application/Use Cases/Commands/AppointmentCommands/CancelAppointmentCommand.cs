using Application.Use_Cases.Commands.AppointmentCommands;
using Domain.Utils;
using MediatR;

namespace Application.Use_Cases.Commands;

public class CancelAppointmentCommand : CancelIdCommand, IRequest<Result<Unit>>
{
    public required string CancellationReason { get; set; }
}