using Domain.Utils;
using MediatR;

namespace Application.Use_Cases.Commands.AppointmentCommands;

public class RescheduleAppointmentCommand : IRequest<Result<Unit>>
{
    public Guid PatientId { get; set; }
    public Guid AppointmentId { get; set; }
    public DateTime? NewDate { get; set; }
    public DateTime? NewStartTime { get; set; }
}