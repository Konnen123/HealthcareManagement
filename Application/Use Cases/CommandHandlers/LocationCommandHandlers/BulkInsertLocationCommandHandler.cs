using Application.Use_Cases.Commands.LocationCommands;
using Domain.Entities;
using Domain.Errors;
using Domain.Repositories;
using Domain.Utils;
using MediatR;

namespace Application.Use_Cases.CommandHandlers.LocationCommandHandlers;

public class BulkInsertLocationCommandHandler : IRequestHandler<BulkInsertLocationCommand, Result<Unit>>
{
    private readonly ILocationRepository _repository;

    public BulkInsertLocationCommandHandler(ILocationRepository repository)
    {
        _repository = repository;
    }
    
    public async Task<Result<Unit>> Handle(BulkInsertLocationCommand request, CancellationToken cancellationToken)
    {
        var locations = CreateLocations(request.MaxFloorNo, request.RoomsPerFloor);
        foreach (var location in locations)
        {
            var resultObj = await _repository.AddAsync(location);
            if (!resultObj.IsSuccess)
            {
                return Result<Unit>.Failure(EntityErrors.CreateFailed(nameof(Location), resultObj.Error!.Description ?? "Something went wrong when bulk inserting locations. You most likely inserted values that were already present in the database."));
            }
        }
        return Result<Unit>.Success(default);
    }
    
    private ICollection<Location> CreateLocations(int maxFloorNo, int roomsPerFloor)
    {
        var locations = new List<Location>();
        for (var floorIndex = 0; floorIndex < maxFloorNo; floorIndex++)
        {
            for (var roomIndex = 1 + roomsPerFloor*floorIndex; roomIndex <= roomsPerFloor*(floorIndex+1); roomIndex++)
            {
                locations.Add(new Location
                {
                    Floor = floorIndex,
                    RoomNo = roomIndex
                });
            }
        }

        return locations;
    }
}