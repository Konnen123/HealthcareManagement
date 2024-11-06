using Application.Use_Cases.Commands.LocationCommands;
using Domain.Entities;
using Domain.Errors;
using Domain.Repositories;
using Domain.Utils;
using MediatR;

namespace Application.Use_Cases.CommandHandlers.LocationCommandHandlers;

public class DeleteLocationCommandHandler : IRequestHandler<DeleteLocationCommand, Result<Unit>>
{
    private readonly ILocationRepository _locationRepository;

    public DeleteLocationCommandHandler(ILocationRepository locationRepository)
    {
        _locationRepository = locationRepository;
    }
    
    public async Task<Result<Unit>> Handle(DeleteLocationCommand request, CancellationToken cancellationToken)
    {
        var resultToPotentialExistingLocation = await _locationRepository.GetAsync(request.Id);
        if(!resultToPotentialExistingLocation.IsSuccess)
        {
            return Result<Unit>.Failure(EntityErrors.NotFound(nameof(Location), request.Id));
        }
        
        var resultToDeleteLocation = await _locationRepository.DeleteAsync(request.Id);
        return resultToDeleteLocation.Match(
            onSuccess: _ => Result<Unit>.Success(Unit.Value),
            onFailure: error => Result<Unit>.Failure(EntityErrors.DeleteFailed(nameof(Location), "Failed to delete location"))
        );
    }
}