using Domain.Entities;
using Domain.Errors;
using Domain.Repositories;
using Domain.Utils;
using Infrastructure.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class LocationRepository : ILocationRepository
{
    private readonly ApplicationDbContext _context;

    public LocationRepository(ApplicationDbContext context)
    {
        _context = context;
    }
    
    public async Task<Result<IEnumerable<Location>>> GetAllAsync()
    {
        try
        {
            var locations = await _context.Locations.ToListAsync();
            return Result<IEnumerable<Location>>.Success(locations);
        }
        catch (Exception e)
        {
            return Result<IEnumerable<Location>>.Failure(EntityErrors.GetFailed(nameof(Location),e.InnerException?.Message ?? "An unexpected error occurred while getting all locations"));
        }
    }

    public async Task<Result<Location>> GetAsync(Guid id)
    {
        try
        {
            var location = await _context.Locations.FindAsync(id);
            return location == null ? Result<Location>.Failure(EntityErrors.NotFound(nameof(Location),id)) : Result<Location>.Success(location);
        }
        catch (Exception e)
        {
            return Result<Location>.Failure(EntityErrors.GetFailed(nameof(Location),e.InnerException?.Message ?? "An unexpected error occurred while getting the location"));
        }
    }

    public async Task<Result<Guid>> AddAsync(Location location)
    {
        try
        {
            await _context.Locations.AddAsync(location);
            await _context.SaveChangesAsync();
            return Result<Guid>.Success(location.LocationId);
        }
        catch (Exception e)
        {
            return Result<Guid>.Failure(EntityErrors.CreateFailed(nameof(Location),e.InnerException?.Message ?? "An unexpected error occurred while creating the location"));
        }
    }

    public async Task<Result<Unit>> UpdateAsync(Location location)
    {
        try
        {
            _context.Locations.Update(location);
            await _context.SaveChangesAsync();
            return Result<Unit>.Success(Unit.Value);
        }
        catch (Exception e)
        {
            return Result<Unit>.Failure(EntityErrors.UpdateFailed(nameof(Location),e.InnerException?.Message ?? "An unexpected error occurred while updating the location"));
        }
    }

    public async Task<Result<Unit>> DeleteAsync(Guid id)
    {
        try
        {
            var locationResult = await GetAsync(id);
            if (!locationResult.IsSuccess)
            {
                return Result<Unit>.Failure(EntityErrors.NotFound(nameof(Location),id));
            }
            
            var location = locationResult.Value!;
            _context.Locations.Remove(location);
            await _context.SaveChangesAsync();

            return Result<Unit>.Success(Unit.Value);
        }
        catch (Exception e)
        {
            return Result<Unit>.Failure(EntityErrors.DeleteFailed(nameof(Location),e.InnerException?.Message ?? "An unexpected error occurred while deleting the location"));
        }
    }

    public async Task<Result<Location>> GetByRoomAndFloorAsync(int room, int floor)
    {
        try
        {
            var location = await _context.Locations.FirstOrDefaultAsync(l => l.RoomNo == room && l.Floor == floor);
            return location == null ? Result<Location>.Failure(LocationErrors.NotFoundByRoomAndFloor(room, floor)) : Result<Location>.Success(location);
        }
        catch (Exception e)
        {
            return Result<Location>.Failure(EntityErrors.GetFailed(nameof(Location),e.InnerException?.Message ?? "An unexpected error occurred while getting the location"));
        }
    }
}