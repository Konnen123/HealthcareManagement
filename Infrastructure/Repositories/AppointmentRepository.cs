using Domain.Entities;
using Domain.Errors;
using Domain.Repositories;
using Domain.Utils;
using Infrastructure.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

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
                return Result<Guid>.Failure(EntityErrors.CreateFailed(nameof(Appointment),e.InnerException?.Message ?? "An unexpected error occurred while creating the appointment"));
            }
        }

        public async Task<Result<Unit>> DeleteAsync(Guid id)
        {
            try
            {
                var appointmentResult = await GetAsync(id);

                if (!appointmentResult.IsSuccess)
                {
                    return Result<Unit>.Failure(EntityErrors.NotFound("Appointment",id));
                }
                var appointment = appointmentResult.Value!;

                context.Appointments.Remove(appointment);
                await context.SaveChangesAsync();

                return Result<Unit>.Success(Unit.Value);
            }
            catch(Exception e)
            {
                return Result<Unit>.Failure(
                    EntityErrors.DeleteFailed(nameof(Appointment), e.InnerException?.Message ?? "An unexpected error occurred while deleting the appointment")
                );
            }
        }

        public async Task<Result<IEnumerable<Appointment>>> GetAllAsync()
        {
            try
            {
                var appointments = await context.Appointments.ToListAsync();
                return appointments.Count == 0
                    ? Result<IEnumerable<Appointment>>.Failure(EntityErrors.GetFailed(nameof(Appointment), "No appointments found."))
                    : Result<IEnumerable<Appointment>>.Success(appointments);
            }
            catch (Exception e)
            {
                return Result<IEnumerable<Appointment>>.Failure(EntityErrors.GetFailed(nameof(Appointment),
                    e.InnerException?.Message ?? "An unexpected error occurred while retrieving appointments."));
            }
        }

        public async Task<Result<Appointment>> GetAsync(Guid id)
        {
            try
            {
                var appointment = await context.Appointments.FindAsync(id);
                return appointment == null ? Result<Appointment>.Failure(EntityErrors.NotFound(nameof(Appointment), id)) : Result<Appointment>.Success(appointment);
            }
            catch (Exception e)
            {
                return Result<Appointment>.Failure(EntityErrors.GetFailed(nameof(Appointment), e.InnerException?.Message ?? $"An unexpected error occurred while retrieving the appointment with id {id}"));
            }
        }
        

        public async Task<Result<Unit>> UpdateAsync(Appointment appointment)
        {
            try
            {
                var existingAppointmentResult = await GetAsync(appointment.Id);

                if (!existingAppointmentResult.IsSuccess)
                {
                    return Result<Unit>.Failure(EntityErrors.NotFound("Appointment", appointment.Id));
                }

                
                var existingAppointment = existingAppointmentResult.Value!;

                existingAppointment.Date = appointment.Date;
                existingAppointment.StartTime = appointment.StartTime;
                existingAppointment.EndTime = appointment.EndTime;
                existingAppointment.UserNotes = appointment.UserNotes;
                existingAppointment.DoctorId = appointment.DoctorId;

                existingAppointment.UpdatedAt = DateTime.UtcNow;
                await context.SaveChangesAsync();

                return Result<Unit>.Success(Unit.Value); 
            }
            catch (Exception e)
            {
                return Result<Unit>.Failure(
                    EntityErrors.UpdateFailed(nameof(Appointment), e.InnerException?.Message ?? "An unexpected error occurred while updating the appointment")
                );
            }

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
