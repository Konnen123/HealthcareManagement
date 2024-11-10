using Application.DTOs;
using Application.Use_Cases.Queries.DailyDoctorSchedulesQueries;
using AutoMapper;
using Domain.Repositories;
using Domain.Utils;
using MediatR;

namespace Application.Use_Cases.QueryHandlers.SchedulesQueryHandlers
{
    public class GetScheduleByIdQueryHandler : IRequestHandler<GetDailyDoctorScheduleByIdQuery, Result<DailyDoctorScheduleDto>>
    {
        private readonly IMapper _mapper;
        private readonly ISchedulesRepository _schedulesRepository;

        public GetScheduleByIdQueryHandler(IMapper mapper, ISchedulesRepository schedulesRepository)
        {
            _mapper = mapper;
            _schedulesRepository = schedulesRepository;
        }

        public async Task<Result<DailyDoctorScheduleDto>> Handle(GetDailyDoctorScheduleByIdQuery request, CancellationToken cancellationToken)
        {
            var resultGetById = await _schedulesRepository.GetAsync(request.Id);
            return resultGetById.Match(
                onSuccess: value => Result<DailyDoctorScheduleDto>.Success(_mapper.Map<DailyDoctorScheduleDto>(value)),
                onFailure: error => Result<DailyDoctorScheduleDto>.Failure(error)
           );
        }

    }
}
