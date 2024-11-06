using Domain.Entities;
using Domain.Utils;
using MediatR;

namespace Domain.Repositories
{
    public interface IAppointmentRepository : IAsyncCrudRepository<Appointment>
    {
        Task<Result<Unit>> CancelAsync(Guid appointmentId, string cancellationReason);
    }
}
