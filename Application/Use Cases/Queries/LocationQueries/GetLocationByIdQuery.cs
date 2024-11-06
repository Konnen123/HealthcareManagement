using Application.DTOs;
using Application.Use_Cases.Commands;
using Domain.Entities;
using Domain.Utils;
using MediatR;

namespace Application.Use_Cases.Queries.LocationQueries;

public class GetLocationByIdQuery : IdCommand, IRequest<Result<LocationDto>>
{
}