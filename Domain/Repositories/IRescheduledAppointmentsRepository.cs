using Domain.Entities;
using Domain.Utils;
using MediatR;

namespace Domain.Repositories;

public interface IRescheduledAppointmentsRepository : IAsyncCrudRepository<AppointmentUpdateRequest>
{
}