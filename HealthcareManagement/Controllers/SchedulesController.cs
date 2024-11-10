using Application.Use_Cases.Commands.SchedulesCommands;
using Application.Use_Cases.Queries.DailyDoctorSchedulesQueries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace HealthcareManagement.Controller;

[Route("api/v1/[controller]")]
[ApiController]
public class SchedulesController : ControllerBase
{
    private readonly IMediator _mediator;

    public SchedulesController(IMediator mediator)
    {
        _mediator = mediator;
    }
    
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetAllSchedules()
    {
        var resultObject = await _mediator.Send(new GetAllSchedulesQuery());
        return resultObject.Match<IActionResult>(
            onSuccess: value => Ok(value),
            onFailure: error => NotFound(error)
        );
    }

    [HttpGet(("{doctorId}/{dayOfWeek}"))]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetScheduleByIdAndDayOfWeek(Guid doctorId, DayOfWeek dayOfWeek)
    {
        var resultObject = await _mediator.Send(new GetScheduleByIdAndDayOfWeekQuery { Id = doctorId, DayOfWeek = dayOfWeek });
        return resultObject.Match<IActionResult>(
           onSuccess: value => Ok(value),
           onFailure: error => NotFound(error)
       );
    }
    [HttpGet(("{id}"))]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetScheduleById(Guid id)
    {
        var resultObject = await _mediator.Send(new GetDailyDoctorScheduleByIdQuery { Id = id });
        return resultObject.Match<IActionResult>(
           onSuccess: value => Ok(value),
           onFailure: error => NotFound(error)
       );
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateSchedule([FromBody] CreateDailyDoctorScheduleCommand command)
    {
        var resultObject = await _mediator.Send(command);
        Console.WriteLine(resultObject.Value);
        Console.WriteLine(resultObject.Error);
        return resultObject.Match<IActionResult>(
             onSuccess: value => CreatedAtAction(nameof(GetScheduleById), new { id = value }, value),
             onFailure: error => BadRequest(error)
         );
    }
    
    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> UpdateSchedule(Guid id, [FromBody] UpdateDailyDoctorScheduleCommand command)
    {
        if(id != command.Id)
        {
            return BadRequest("The ID in the URL does not match the ID in the request body.");
        }

        
        var resultObject = await _mediator.Send(command);
        return resultObject.Match<IActionResult>(
           onSuccess: unit => NoContent(),
           onFailure: error => BadRequest(error)
       );
    }

    [HttpDelete("{doctorId}/{dayOfWeek}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteSchedule(Guid doctorId, DayOfWeek dayOfWeek)
    {

        var resultObject = await _mediator.Send(new DeleteDailyDoctorScheduleByDoctorIdAndDayOfWeekCommand { DoctorId = doctorId, DayOfWeek = dayOfWeek });
        return resultObject.Match<IActionResult>(
            onSuccess: _ => NoContent(),
            onFailure: error => NotFound(error)
        );
         
    }

    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteSchedule(Guid id)
    {

        var resultObject = await _mediator.Send(new DeleteDailyDoctorScheduleByIdCommand { Id = id });
        return resultObject.Match<IActionResult>(
            onSuccess: _ => NoContent(),
            onFailure: error => NotFound(error)
        );

    }
}
