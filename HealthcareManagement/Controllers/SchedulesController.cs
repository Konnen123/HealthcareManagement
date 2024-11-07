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
    public async Task<IActionResult> GetAllSchedules()
    {
        return Ok("Schedules GET ALL");
    }
    
    [HttpGet("{id}")]
    public async Task<IActionResult> GetScheduleById(Guid id)
    {
        return Ok($"Schedules GET By Id: {id}");
    }
    
    [HttpPost]
    public async Task<IActionResult> CreateSchedule()
    {
        return Ok("Schedules POST");
    }
    
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateSchedule(Guid id)
    {
        return Ok($"Schedules PUT: {id}");
    }
    
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteSchedule(Guid id)
    {
        return Ok($"Schedules DELETE: {id}");
    }
}
