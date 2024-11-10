using Domain.Entities;
using Domain.Errors;
using Domain.Repositories;
using Domain.Utils;
using Infrastructure.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;


namespace Infrastructure.Repositories
{
    public class SchedulesRepository : ISchedulesRepository
    {
        private readonly ApplicationDbContext _context;

        public SchedulesRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Result<Guid>> AddAsync(DailyDoctorSchedule dailyDoctorSchedule)
        {
            try
            {
                await _context.DailyDoctorSchedules.AddAsync(dailyDoctorSchedule);
                await _context.SaveChangesAsync();
                return Result<Guid>.Success(dailyDoctorSchedule.DailyDoctorScheduleId);
            }
            catch (Exception e)
            {
                return Result<Guid>.Failure(EntityErrors.CreateFailed(nameof(DailyDoctorSchedule), e.InnerException?.Message ?? "An unexpected error occurred while creating the schedule"));
            }
        }

        public async Task<Result<Unit>> DeleteAsync(Guid id,DayOfWeek dayOfWeek)
        {
            var getScheduleResult = await GetAsync(id ,dayOfWeek);
            if (!getScheduleResult.IsSuccess)
            {
                return Result<Unit>.Failure(EntityErrors.NotFound(nameof(DailyDoctorSchedule), id));
            }

            var schedule = getScheduleResult.Value!;
            _context.DailyDoctorSchedules.Remove(schedule);
            await _context.SaveChangesAsync();

            return Result<Unit>.Success(Unit.Value);
        }

        public async Task<Result<Unit>> DeleteAsync(Guid id)
        {
            var getScheduleResult = await GetAsync(id);
            if (!getScheduleResult.IsSuccess)
            {
                return Result<Unit>.Failure(EntityErrors.NotFound(nameof(DailyDoctorSchedule), id));
            }

            var schedule = getScheduleResult.Value!;
            _context.DailyDoctorSchedules.Remove(schedule);
            await _context.SaveChangesAsync();

            return Result<Unit>.Success(Unit.Value);
        }
        public async Task<Result<IEnumerable<DailyDoctorSchedule>>> GetAllAsync()
        {
            try
            {
                var dailyDoctorSchedules = await _context.DailyDoctorSchedules.ToListAsync();
                return Result<IEnumerable<DailyDoctorSchedule>>.Success(dailyDoctorSchedules);
            }
            catch (Exception e)
            {
                return Result<IEnumerable<DailyDoctorSchedule>>.Failure(EntityErrors.GetFailed(nameof(DailyDoctorSchedule), e.InnerException?.Message ?? "An unexpected error occurred while getting all schedules"));
            }
        }
        public async Task<Result<DailyDoctorSchedule>> GetAsync(Guid id)
        {
            try
            {
                var dailyDoctorSchedule = await _context.DailyDoctorSchedules.FirstOrDefaultAsync(s => s.DailyDoctorScheduleId == id);

                return dailyDoctorSchedule == null
                    ? Result<DailyDoctorSchedule>.Failure(EntityErrors.NotFound(nameof(DailyDoctorSchedule), id))
                    : Result<DailyDoctorSchedule>.Success(dailyDoctorSchedule);
            }
            catch (Exception e)
            {
                return Result<DailyDoctorSchedule>.Failure(
                    EntityErrors.GetFailed(nameof(DailyDoctorSchedule),
                    e.InnerException?.Message ?? "An unexpected error occurred while getting the schedule"));
            }
        }
        public async Task<Result<DailyDoctorSchedule>> GetAsync(Guid id, DayOfWeek dayOfWeek)
        {
            try
            {
                var dailyDoctorSchedule = await _context.DailyDoctorSchedules.FirstOrDefaultAsync(l =>l.DoctorId == id && l.DayOfWeek == dayOfWeek);

                return dailyDoctorSchedule == null
                    ? Result<DailyDoctorSchedule>.Failure(EntityErrors.NotFound(nameof(DailyDoctorSchedule), id, dayOfWeek))
                    : Result<DailyDoctorSchedule>.Success(dailyDoctorSchedule);
            }
            catch (Exception e)
            {
                return Result<DailyDoctorSchedule>.Failure(
                    EntityErrors.GetFailed(nameof(DailyDoctorSchedule),
                    e.InnerException?.Message ?? "An unexpected error occurred while getting the schedule"));
            }

        }

        public async Task<Result<Unit>> UpdateAsync(DailyDoctorSchedule dailyDoctorSchedule)
        {
            try
            {
            
                var existingScheduleResult = await GetAsync(dailyDoctorSchedule.DoctorId,dailyDoctorSchedule.DayOfWeek);

                if (!existingScheduleResult.IsSuccess)
                {
                    return Result<Unit>.Failure(EntityErrors.NotFound("DailyDoctorSchedule", dailyDoctorSchedule.DailyDoctorScheduleId));
                }
                var existingSchedule = existingScheduleResult.Value;

                existingSchedule.LocationId = dailyDoctorSchedule.LocationId;
                existingSchedule.StartingTime = dailyDoctorSchedule.StartingTime;
                existingSchedule.EndingTime = dailyDoctorSchedule.EndingTime;
                existingSchedule.SlotDurationMinutes = dailyDoctorSchedule.SlotDurationMinutes;

                await _context.SaveChangesAsync();

                return Result<Unit>.Success(Unit.Value);
            }
            catch (Exception e)
            {
                return Result<Unit>.Failure(EntityErrors.UpdateFailed(nameof(DailyDoctorSchedule), e.InnerException?.Message ?? "An unexpected error occurred while updating the schedule"));
            }
        }
    }
}
