using Domain.Utils;
using MediatR;

namespace Application.Use_Cases.Commands.SchedulesCommands
{
    public class CreateDailyDoctorScheduleCommand : BaseDailyDoctorScheduleCommand, IRequest<Result<Guid>>;
}
