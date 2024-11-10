using Application.Use_Cases.Queries.DailyDoctorSchedulesQueries;
using Domain.Utils;
using MediatR;

namespace Application.Use_Cases.Commands.SchedulesCommands
{
    public class DeleteDailyDoctorScheduleByDoctorIdAndDayOfWeekCommand : IRequest<Result<Unit>>
    {
       public Guid DoctorId { get; set; }
       public DayOfWeek DayOfWeek { get; set; }
    }
}
