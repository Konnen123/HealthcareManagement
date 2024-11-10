using Domain.Entities;
using Domain.Errors;
using Domain.Repositories;
using Domain.Utils;
using Infrastructure.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class RescheduledAppointmentsRepository : IRescheduledAppointmentsRepository
{
    private readonly ApplicationDbContext _context;

    public RescheduledAppointmentsRepository(ApplicationDbContext context)
    {
        _context = context;
    }
    
    public async Task<Result<IEnumerable<AppointmentUpdateRequest>>> GetAllAsync()
    {
        try
        {
            var appointmentUpdateRequests = await _context.AppointmentUpdateRequests.ToListAsync();
            return Result<IEnumerable<AppointmentUpdateRequest>>.Success(appointmentUpdateRequests);
        }
        catch (Exception e)
        {
            return Result<IEnumerable<AppointmentUpdateRequest>>.Failure(EntityErrors.GetFailed(nameof(AppointmentUpdateRequest), e.InnerException?.Message ?? "Something went wrong in getting appointment requests"));
        }
    }

    public async Task<Result<AppointmentUpdateRequest>> GetAsync(Guid id)
    {
        try
        {
            var appointmentUpdateRequest = await _context.AppointmentUpdateRequests.FirstOrDefaultAsync(x => x.AppointmentId == id);
            return appointmentUpdateRequest == null ? Result<AppointmentUpdateRequest>.Failure(EntityErrors.NotFound(nameof(AppointmentUpdateRequest), id)) : Result<AppointmentUpdateRequest>.Success(appointmentUpdateRequest);
        }
        catch (Exception e)
        {
            return Result<AppointmentUpdateRequest>.Failure(EntityErrors.GetFailed(nameof(AppointmentUpdateRequest), e.InnerException?.Message ?? "Something went wrong in getting appointment request"));
        }
    }

    public async Task<Result<Guid>> AddAsync(AppointmentUpdateRequest appointmentUpdateRequest)
    {
        try
        {
            await _context.AppointmentUpdateRequests.AddAsync(appointmentUpdateRequest);
            await _context.SaveChangesAsync();
            return Result<Guid>.Success(appointmentUpdateRequest.Id);
        }
        catch (Exception e)
        {
            return Result<Guid>.Failure(EntityErrors.CreateFailed(nameof(AppointmentUpdateRequest), e.InnerException?.Message ?? "Something went wrong in creating appointment request"));
        }
    }

    public Task<Result<Unit>> UpdateAsync(AppointmentUpdateRequest _)
    {
        throw new NotImplementedException();
    }

    public Task<Result<Unit>> DeleteAsync(Guid id)
    {
        try
        {
            var appointmentUpdateRequest = _context.AppointmentUpdateRequests.FirstOrDefault(x => x.Id == id);
            if (appointmentUpdateRequest == null)
            {
                return Task.FromResult(Result<Unit>.Failure(EntityErrors.NotFound(nameof(AppointmentUpdateRequest), id)));
            }
            _context.AppointmentUpdateRequests.Remove(appointmentUpdateRequest);
            _context.SaveChanges();
            return Task.FromResult(Result<Unit>.Success(default));
        }
        catch (Exception e)
        {
            return Task.FromResult(Result<Unit>.Failure(EntityErrors.DeleteFailed(nameof(AppointmentUpdateRequest), e.InnerException?.Message ?? "Something went wrong in deleting appointment request")));
        }
    }
}