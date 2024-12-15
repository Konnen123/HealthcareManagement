using Application.DTOs.UserDto;
using Application.Use_Cases.Queries.UserQueries;
using AutoMapper;
using Domain.Repositories;
using Domain.Utils;
using MediatR;

namespace Application.Use_Cases.QueryHandlers.UserQueryHandlers
{
    public class GetAllDoctorsQueryHandler : IRequestHandler<GetAllDoctorsQuery, Result<List<DoctorDto>>>
    {
        private readonly IMapper mapper;
        private readonly IDoctorsRepository doctorsRepository;

        public GetAllDoctorsQueryHandler(IMapper mapper, IDoctorsRepository doctorsRepository)
        {
            this.mapper = mapper;
            this.doctorsRepository = doctorsRepository;
        }

        public async Task<Result<List<DoctorDto>>> Handle(GetAllDoctorsQuery request, CancellationToken cancellationToken)
        {
            var result = await doctorsRepository.GetAllDoctors(cancellationToken);

            if(!result.IsSuccess)
            {
                return Result<List<DoctorDto>>.Failure(result.Error!);
            }

            var doctorsDto = result.Value!
                .Select(x => mapper.Map<DoctorDto>(x))
                .ToList();

            return Result<List<DoctorDto>>.Success(doctorsDto);
        }
    }
}
