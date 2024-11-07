using Application.DTOs;
using Application.Use_Cases.Queries.LocationQueries;
using AutoMapper;
using Domain.Repositories;
using Domain.Utils;
using MediatR;

namespace Application.Use_Cases.QueryHandlers.LocationQueryHandlers;

public class GetAllLocationsQueryHandler : IRequestHandler<GetAllLocationsQuery, Result<ICollection<LocationDto>>>
{
    private readonly IMapper _mapper;
    private readonly ILocationRepository _locationRepository;

    public GetAllLocationsQueryHandler(IMapper mapper, ILocationRepository locationRepository)
    {
        _mapper = mapper;
        _locationRepository = locationRepository;
    }
    
    
    public async Task<Result<ICollection<LocationDto>>> Handle(GetAllLocationsQuery request, CancellationToken cancellationToken)
    {
        var resultToGetAllLocations = await _locationRepository.GetAllAsync();
        
        return resultToGetAllLocations.Match(
            onSuccess: value => Result<ICollection<LocationDto>>.Success(_mapper.Map<ICollection<LocationDto>>(value)),
            onFailure: error => Result<ICollection<LocationDto>>.Failure(error)
        );
    }
}