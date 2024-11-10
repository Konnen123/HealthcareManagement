using Domain.Utils;
using MediatR;

namespace Application.Use_Cases.Commands.SchedulesCommands
{
    public class DeleteDailyDoctorScheduleByIdCommand : IdCommand, IRequest<Result<Unit>>
    {
    }
}
