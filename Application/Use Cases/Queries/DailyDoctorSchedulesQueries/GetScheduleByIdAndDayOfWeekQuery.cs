using Application.DTOs;
using Application.Use_Cases.Commands;
using Domain.Utils;
using MediatR;

namespace Application.Use_Cases.Queries.DailyDoctorSchedulesQueries
{
    public class GetScheduleByIdAndDayOfWeekQuery : IdCommand, IRequest<Result<DailyDoctorScheduleDto>>
    {
        public DayOfWeek DayOfWeek { get; set; }
    }
}
