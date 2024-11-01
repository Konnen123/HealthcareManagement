using Domain.Entities;
using Domain.Utils;
using MediatR;

namespace Domain.Repositories
{
    public interface IAppointmentRepository
    {
        Task<Result<IEnumerable<Appointment>>> GetAllAsync();
        Task<Result<Appointment>> GetAsync(Guid id);
        Task<Result<Guid>> AddAsync(Appointment appointment);
        Task<Result<Unit>> UpdateAsync(Appointment appointment);
        Task<Result<Unit>> DeleteAsync(Guid id);
        Task<Result<Unit>> CancelAsync(Guid appointmentId, string cancellationReason);
    }
}
