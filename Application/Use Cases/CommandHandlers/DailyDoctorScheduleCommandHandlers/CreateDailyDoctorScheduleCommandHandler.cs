using Application.Use_Cases.Commands.SchedulesCommands;
using AutoMapper;
using Domain.Entities;
using Domain.Repositories;
using Domain.Utils;
using MediatR;

namespace Application.Use_Cases.CommandHandlers.AppointmentCommandHandlers;

public class CreateDailyDoctorScheduleCommandHandler : IRequestHandler<CreateDailyDoctorScheduleCommand, Result<Guid>>
{
    private readonly IMapper _mapper;
    private readonly ISchedulesRepository _repository;

    public CreateDailyDoctorScheduleCommandHandler(IMapper mapper, ISchedulesRepository repository)
    {
        _mapper = mapper;
        _repository = repository;
    }


    public async Task<Result<Guid>> Handle(CreateDailyDoctorScheduleCommand request, CancellationToken cancellationToken)
    {
        var dailyDoctorSchedule = _mapper.Map<DailyDoctorSchedule>(request);
        var resultCreateSchedule = await _repository.AddAsync(dailyDoctorSchedule);
        return resultCreateSchedule.Match<Result<Guid>>(
            onSuccess: value => Result<Guid>.Success(value),
            onFailure: error => Result<Guid>.Failure(error));
    }
}