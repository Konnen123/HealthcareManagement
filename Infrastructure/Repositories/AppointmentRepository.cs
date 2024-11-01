using Domain.Entities;
using Domain.Errors;
using Domain.Repositories;
using Domain.Utils;
using Infrastructure.Persistence;
using MediatR;

namespace Infrastructure.Repositories
{
    public class AppointmentRepository : IAppointmentRepository
    {
        private readonly ApplicationDbContext context;

        public AppointmentRepository(ApplicationDbContext context)
        {
            this.context = context;
        }

        public async Task<Result<Guid>> AddAsync(Appointment appointment)
        {
            try
            {
                await context.Appointments.AddAsync(appointment);
                await context.SaveChangesAsync();
                return Result<Guid>.Success(appointment.Id);
            }
            catch (Exception e)
            {
                return Result<Guid>.Failure(AppointmentErrors.CreateFailed(e.InnerException?.Message ?? "An unexpected error occurred while creating the appointment"));
            }
        }

        public async Task<Result<Unit>> DeleteAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public async Task<Result<IEnumerable<Appointment>>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public async Task<Result<Appointment>> GetAsync(Guid id)
        {
            try
            {
                var appointment = await context.Appointments.FindAsync(id);
                return appointment == null ? Result<Appointment>.Failure(AppointmentErrors.NotFound(id)) : Result<Appointment>.Success(appointment);
            }
            catch (Exception e)
            {
                return Result<Appointment>.Failure(AppointmentErrors.GetFailed(e.InnerException?.Message ?? $"An unexpected error occurred while retrieving the appointment with id {id}"));
            }
        }
        

        public async Task<Result<Unit>> UpdateAsync(Appointment appointment)
        {
            throw new NotImplementedException();
        }

        public async Task<Result<Unit>> CancelAsync(Guid AppointmentId, string cancellationReason)
        {
            try
            {
                var resultObject = await GetAsync(AppointmentId);
                var appointment = resultObject.Value!;
                appointment.CanceledAt = DateTime.UtcNow;
                appointment.CancellationReason = cancellationReason;
                appointment.UpdatedAt = DateTime.UtcNow;
                await context.SaveChangesAsync();
                return Result<Unit>.Success(default);
            }
            catch (Exception e)
            {
                return Result<Unit>.Failure(
                    AppointmentErrors.CancelFailed(e.InnerException?.Message ?? "Cancel failed"));
            }
        }
    }
}
