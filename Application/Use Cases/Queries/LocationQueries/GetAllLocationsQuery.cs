using Application.DTOs;
using Domain.Utils;
using MediatR;

namespace Application.Use_Cases.Queries.LocationQueries;

public class GetAllLocationsQuery : IRequest<Result<ICollection<LocationDto>>>
{
}