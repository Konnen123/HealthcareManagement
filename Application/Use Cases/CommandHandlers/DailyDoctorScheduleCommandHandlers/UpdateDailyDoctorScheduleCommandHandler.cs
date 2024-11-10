using Application.Use_Cases.Commands.SchedulesCommands;
using AutoMapper;
using Domain.Entities;
using Domain.Repositories;
using Domain.Utils;
using MediatR;

namespace Application.Use_Cases.CommandHandlers.AppointmentCommandHandlers;

public class UpdateDailyDoctorScheduleCommandHandler : IRequestHandler<UpdateDailyDoctorScheduleCommand, Result<Unit>>
{
    private readonly IMapper _mapper;
    private readonly ISchedulesRepository _repository;

    public UpdateDailyDoctorScheduleCommandHandler(IMapper mapper, ISchedulesRepository repository)
    {
        _mapper = mapper;
        _repository = repository;
    }


    public async Task<Result<Unit>> Handle(UpdateDailyDoctorScheduleCommand request, CancellationToken cancellationToken)
    {
        var dailyDoctorSchedule = _mapper.Map<DailyDoctorSchedule>(request);
        var resultUpdateSchedule = await _repository.UpdateAsync(dailyDoctorSchedule);
        return resultUpdateSchedule.Match<Result<Unit>>(
               onSuccess: value => Result<Unit>.Success(Unit.Value),
               onFailure: error => Result<Unit>.Failure(error));
    }
}
