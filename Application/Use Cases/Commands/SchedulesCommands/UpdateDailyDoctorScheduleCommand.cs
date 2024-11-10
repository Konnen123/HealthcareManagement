using Domain.Utils;
using MediatR;

namespace Application.Use_Cases.Commands.SchedulesCommands
{
    public class UpdateDailyDoctorScheduleCommand : BaseDailyDoctorScheduleCommand, IRequest<Result<Unit>>
    {
        public Guid DailyDoctorScheduleId { get; set; }
    }
}
