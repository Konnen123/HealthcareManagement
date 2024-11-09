using Application.DTOs;
using Application.Utils;
using Domain.Utils;
using MediatR;

namespace Application.Use_Cases.Queries.LocationQueries;

public class GetFilteredLocationsQuery : IRequest<Result<PagedResult<LocationDto>>>
{
    public int Page { get; set; }
    public int PageSize { get; set; }
}