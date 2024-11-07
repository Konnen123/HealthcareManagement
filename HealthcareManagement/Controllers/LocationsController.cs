using Application.Use_Cases.Commands.LocationCommands;
using Application.Use_Cases.Queries.LocationQueries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace HealthcareManagement.Controllers;

[Route("api/v1/[controller]")]
[ApiController]
public class LocationsController : ControllerBase
{
    private readonly IMediator _mediator;

    public LocationsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetAllLocations()
    {
        var resultObject = await _mediator.Send(new GetAllLocationsQuery());
        return resultObject.Match<IActionResult>(
            onSuccess: value => Ok(value),
            onFailure: error => BadRequest(error)
        );
    }
    
    [HttpGet("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetLocationById(Guid id)
    {
        var resultObject = await _mediator.Send(new GetLocationByIdQuery { Id = id });
        return resultObject.Match<IActionResult>(
            onSuccess: value => Ok(value),
            onFailure: error => NotFound(error)
        );
    }
    
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateLocation([FromBody] CreateLocationCommand command)
    {
        var resultObject = await _mediator.Send(command);
        return resultObject.Match<IActionResult>(
            onSuccess: value => CreatedAtAction(nameof(GetLocationById), new { id = value }, value),
            onFailure: error => BadRequest(error)
        );
    }
    
    [HttpDelete("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteLocation(Guid id)
    {
        var resultObject = await _mediator.Send(new DeleteLocationCommand { Id = id });
        return resultObject.Match<IActionResult>(
            onSuccess: _ => NoContent(),
            onFailure: error => NotFound(error)
        );
    }
    
    [HttpPost("bulk-insert")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> BulkInsertLocations([FromQuery] int maxFloorNo = 3, [FromQuery] int roomsPerFloor = 25)
    {
        var command = new BulkInsertLocationCommand { MaxFloorNo = maxFloorNo, RoomsPerFloor = roomsPerFloor };
        var resultObject = await _mediator.Send(command);
        return resultObject.Match<IActionResult>(
            onSuccess: value => Created(string.Empty, value),
            onFailure: error => BadRequest(error)
        );
    }
    
    // [HttpGet("paginated")]
    // [ProducesResponseType(StatusCodes.Status200OK)]
    // [ProducesResponseType(StatusCodes.Status400BadRequest)]
    // public async Task<IActionResult> GetPaginatedLocations([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
    // {
    //     var resultObject = await _mediator.Send(new GetPaginatedLocationsQuery { Page = page, PageSize = pageSize });
    //     return resultObject.Match<IActionResult>(
    //         onSuccess: value => Ok(value),
    //         onFailure: error => BadRequest(error)
    //     );
    // }
}