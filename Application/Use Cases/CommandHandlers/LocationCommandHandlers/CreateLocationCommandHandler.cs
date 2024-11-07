using Application.Use_Cases.Commands.LocationCommands;
using AutoMapper;
using Domain.Errors;
using Domain.Repositories;
using Domain.Utils;
using MediatR;

namespace Application.Use_Cases.CommandHandlers.LocationCommandHandlers;

public class CreateLocationCommandHandler : IRequestHandler<CreateLocationCommand, Result<Guid>>
{
    private readonly IMapper _mapper;
    private readonly ILocationRepository _locationRepository;

    public CreateLocationCommandHandler(IMapper mapper, ILocationRepository locationRepository)
    {
        _mapper = mapper;
        _locationRepository = locationRepository;
    }
    public async Task<Result<Guid>> Handle(CreateLocationCommand request, CancellationToken cancellationToken)
    {
        var location = _mapper.Map<Domain.Entities.Location>(request);
        var resultToGetRoom = await _locationRepository.GetByRoomAndFloorAsync(location.RoomNo, location.Floor);
        if(resultToGetRoom.Value is not  null)
            return Result<Guid>.Failure(LocationErrors.RoomAlreadyExists(location.RoomNo, location.Floor));
        
        var resultToLocationAdd = await _locationRepository.AddAsync(location);
        return resultToLocationAdd.Match<Result<Guid>>(
            onSuccess: value => Result<Guid>.Success(value),
            onFailure: error => Result<Guid>.Failure(error));
    }
}