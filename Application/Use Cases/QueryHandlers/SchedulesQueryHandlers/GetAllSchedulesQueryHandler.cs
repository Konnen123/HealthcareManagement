using Application.DTOs;
using Application.Use_Cases.Queries.DailyDoctorSchedulesQueries;
using AutoMapper;
using Domain.Repositories;
using Domain.Utils;
using MediatR;

namespace Application.Use_Cases.QueryHandlers.SchedulesQueryHandlers;
public class GetAllSchedulesQueryHandler : IRequestHandler<GetAllSchedulesQuery, Result<ICollection<DailyDoctorScheduleDto>>>
{
    private readonly IMapper _mapper;
    private readonly ISchedulesRepository _schedulesRepository;

    public GetAllSchedulesQueryHandler(IMapper mapper, ISchedulesRepository schedulesRepository)
    {
        _mapper = mapper;
        _schedulesRepository = schedulesRepository;
    }

    public async Task<Result<ICollection<DailyDoctorScheduleDto>>> Handle(GetAllSchedulesQuery request, CancellationToken cancellationToken)
    {
        var resultGetAll = await _schedulesRepository.GetAllAsync();

        return resultGetAll.Match(
            onSuccess: value => Result<ICollection<DailyDoctorScheduleDto>>.Success(_mapper.Map<ICollection<DailyDoctorScheduleDto>>(value)),
            onFailure: error => Result<ICollection<DailyDoctorScheduleDto>>.Failure(error)
        );
    }
}