using Domain.Entities;
using Domain.Utils;

namespace Domain.Repositories;

public interface ILocationRepository : IAsyncCrudRepository<Location>
{
    public Task<Result<Location>> GetByRoomAndFloorAsync(int room, int floor);
}