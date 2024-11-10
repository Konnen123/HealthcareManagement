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
        private readonly IMapper _mapper;
        public DeleteDailyDoctorScheduleByIdCommandHandler(ISchedulesRepository schedulesRepository, IMapper mapper)
        {
            _schedulesRepository = schedulesRepository;
            _mapper = mapper;

        }
        public async Task<Result<Unit>> Handle(DeleteDailyDoctorScheduleByIdCommand request, CancellationToken cancellationToken)
        {
            var resultDeleteSchedule = await _schedulesRepository.DeleteAsync(request.Id);

            return resultDeleteSchedule.Match(
            onSuccess: _ => Result<Unit>.Success(Unit.Value),
            onFailure: error => Result<Unit>.Failure(EntityErrors.DeleteFailed(nameof(Location), "Failed to delete the daily doctor schedule."))
            );
        }
    }
}
