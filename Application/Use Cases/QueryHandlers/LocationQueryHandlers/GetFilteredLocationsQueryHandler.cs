using Application.DTOs;
using Application.Use_Cases.Queries.LocationQueries;
using Application.Utils;
using AutoMapper;
using Domain.Entities;
using Domain.Errors;
using Domain.Repositories;
using Domain.Utils;
using Gridify;
using MediatR;

namespace Application.Use_Cases.QueryHandlers.LocationQueryHandlers;

public class GetFilteredLocationsQueryHandler : IRequestHandler<GetFilteredLocationsQuery, Result<PagedResult<LocationDto>>>
{
    private readonly IMapper _mapper;
    private readonly ILocationRepository _locationRepository;

    public GetFilteredLocationsQueryHandler(IMapper mapper, ILocationRepository locationRepository)
    {
        _mapper = mapper;
        _locationRepository = locationRepository;
    }

    public async Task<Result<PagedResult<LocationDto>>> Handle(GetFilteredLocationsQuery request, CancellationToken cancellationToken)
    {
        var locations = await _locationRepository.GetAllAsync();
        if (!locations.IsSuccess)
        {
            return Result<PagedResult<LocationDto>>.Failure(EntityErrors.GetFailed(nameof(Location), locations.Error?.Description ?? "Something went wrong while fetching locations"));
        }
        var query = locations.Value!.AsQueryable();
        var pagedLocations = query.ApplyPaging(request.Page, request.PageSize);
        var locationDtos = _mapper.Map<List<LocationDto>>(pagedLocations);
        var pagedResult = new PagedResult<LocationDto>(locationDtos, query.Count());
        return Result<PagedResult<LocationDto>>.Success(pagedResult);
    }
}