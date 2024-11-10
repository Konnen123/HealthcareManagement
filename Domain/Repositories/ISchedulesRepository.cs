using Domain.Entities;
using Domain.Utils;
using MediatR;

namespace Domain.Repositories
{
    public interface ISchedulesRepository :IAsyncCrudRepository<DailyDoctorSchedule>
    {
        Task<Result<DailyDoctorSchedule>> GetAsync(Guid id, DayOfWeek dayOfWeek);
        Task<Result<Unit>> DeleteAsync(Guid id, DayOfWeek dayOfWeek);
    }
}
