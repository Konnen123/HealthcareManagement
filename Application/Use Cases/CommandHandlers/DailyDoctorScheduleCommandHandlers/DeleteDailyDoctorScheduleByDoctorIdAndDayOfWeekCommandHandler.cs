using Application.Use_Cases.Commands.SchedulesCommands;
using AutoMapper;
using Domain.Entities;
using Domain.Errors;
using Domain.Repositories;
using Domain.Utils;
using MediatR;

namespace Application.Use_Cases.CommandHandlers.DailyDoctorScheduleCommandHandlers
{
    public class DeleteDailyDoctorScheduleByDoctorIdAndDayOfWeekCommandHandler : IRequestHandler<DeleteDailyDoctorScheduleByDoctorIdAndDayOfWeekCommand, Result<Unit>>
    {
        private readonly ISchedulesRepository _schedulesRepository;
        public DeleteDailyDoctorScheduleByDoctorIdAndDayOfWeekCommandHandler(ISchedulesRepository schedulesRepository)
        {
            _schedulesRepository = schedulesRepository;  
        }
        public async Task<Result<Unit>> Handle(DeleteDailyDoctorScheduleByDoctorIdAndDayOfWeekCommand request, CancellationToken cancellationToken)
        {
            var resultDeleteSchedule = await _schedulesRepository.DeleteAsync(request.DoctorId, request.DayOfWeek);

            if (!resultDeleteSchedule.IsSuccess)
            {
                return Result<Unit>.Failure(EntityErrors.NotFound(nameof(DailyDoctorSchedule), request.DoctorId, request.DayOfWeek));
            }

            return resultDeleteSchedule.Match(
            onSuccess: _ => Result<Unit>.Success(Unit.Value),
            onFailure: error => Result<Unit>.Failure(EntityErrors.DeleteFailed(nameof(DailyDoctorSchedule), "Failed to delete the daily doctor schedule."))
            );
        }
    }
}
