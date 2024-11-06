using Application.DTOs;
using Application.Use_Cases.Queries.LocationQueries;
using AutoMapper;
using Domain.Repositories;
using Domain.Utils;
using MediatR;

namespace Application.Use_Cases.QueryHandlers.LocationQueryHandlers;

public class GetLocationByIdQueryHandler : IRequestHandler<GetLocationByIdQuery, Result<LocationDto>>
{
    private readonly IMapper _mapper;
    private readonly ILocationRepository _locationRepository;

    public GetLocationByIdQueryHandler(IMapper mapper, ILocationRepository locationRepository)
    {
        _mapper = mapper;
        _locationRepository = locationRepository;
    }
    
    public async Task<Result<LocationDto>> Handle(GetLocationByIdQuery request, CancellationToken cancellationToken)
    {
        var resultToGetLocationById = await _locationRepository.GetAsync(request.Id);
        return resultToGetLocationById.Match<Result<LocationDto>>(
            onSuccess: value => Result<LocationDto>.Success(_mapper.Map<LocationDto>(value)),
            onFailure: error => Result<LocationDto>.Failure(error)
        );
    }
}