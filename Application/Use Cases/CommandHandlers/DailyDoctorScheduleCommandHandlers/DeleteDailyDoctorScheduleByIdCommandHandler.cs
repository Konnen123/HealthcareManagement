using Application.Use_Cases.Commands.SchedulesCommands;
using AutoMapper;
using Domain.Entities;
using Domain.Errors;
using Domain.Repositories;
using Domain.Utils;
using MediatR;

namespace Application.Use_Cases.CommandHandlers.DailyDoctorScheduleCommandHandlers
{
    public class DeleteDailyDoctorScheduleByIdCommandHandler : IRequestHandler<DeleteDailyDoctorScheduleByIdCommand, Result<Unit>>
    {
        private readonly ISchedulesRepository _schedulesRepository;
        public DeleteDailyDoctorScheduleByIdCommandHandler(ISchedulesRepository schedulesRepository)
        {
            _schedulesRepository = schedulesRepository;           
        }
        public async Task<Result<Unit>> Handle(DeleteDailyDoctorScheduleByIdCommand request, CancellationToken cancellationToken)
        {
            var resultDeleteSchedule = await _schedulesRepository.DeleteAsync(request.Id);

            if (!resultDeleteSchedule.IsSuccess)
            {
                return Result<Unit>.Failure(EntityErrors.NotFound(nameof(DailyDoctorSchedule), request.Id));
            }


            return resultDeleteSchedule.Match(
            onSuccess: _ => Result<Unit>.Success(Unit.Value),
            onFailure: error => Result<Unit>.Failure(EntityErrors.DeleteFailed(nameof(DailyDoctorSchedule), "Failed to delete the daily doctor schedule."))
            );
        }
    }
}
