using Domain.Entities;
using Domain.Utils;
using MediatR;

namespace Domain.Repositories
{
    public interface IAppointmentRepository : ICrudRepository<Appointment>
    {
        Task<Result<Unit>> CancelAsync(Guid appointmentId, string cancellationReason);
    }
}
