using Application.DTOs;
using Application.Use_Cases.Queries.DailyDoctorSchedulesQueries;
using AutoMapper;
using Domain.Repositories;
using Domain.Utils;
using MediatR;

namespace Application.Use_Cases.QueryHandlers.SchedulesQueryHandlers
{
    public class GetScheduleByIdAndDayOfWeekQueryHandler : IRequestHandler<GetScheduleByIdAndDayOfWeekQuery, Result<DailyDoctorScheduleDto>>
    {
        private readonly IMapper _mapper;
        private readonly ISchedulesRepository _schedulesRepository;

        public GetScheduleByIdAndDayOfWeekQueryHandler(IMapper mapper, ISchedulesRepository schedulesRepository)
        {
            _mapper = mapper;
            _schedulesRepository = schedulesRepository;
        }

        public async Task<Result<DailyDoctorScheduleDto>> Handle(GetScheduleByIdAndDayOfWeekQuery request, CancellationToken cancellationToken)
        {
            var resultGetById = await _schedulesRepository.GetAsync(request.Id, request.DayOfWeek);
            return resultGetById.Match(
                onSuccess: value => Result<DailyDoctorScheduleDto>.Success(_mapper.Map<DailyDoctorScheduleDto>(value)),
                onFailure: error => Result<DailyDoctorScheduleDto>.Failure(error)
           );
        }

    }
}
