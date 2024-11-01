using Domain.Utils;
using MediatR;

namespace Application.Use_Cases.Commands;

public class CancelAppointmentCommand : IRequest<Result<Unit>>
{
    public Guid PatientId { get; set; }
    public Guid AppointmentId { get; set; } //al programarii anulate
    public string CancellationReason { get; set; }
}