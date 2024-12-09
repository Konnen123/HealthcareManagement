using Application.DTOs;
using Application.Use_Cases.Commands;
using Application.Use_Cases.Commands.AppointmentCommands;
using Application.Use_Cases.Queries.AppointmentQueries;
using Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using Swashbuckle.AspNetCore.Annotations;

namespace HealthcareManagement.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class AppointmentsController : ODataController
    {
        private readonly IMediator mediator;

        public AppointmentsController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Authorize(Policy = "PACIENT")]
        public async Task<IActionResult> CreateAppointment([FromBody] CreateAppointmentCommand command)
        {

            var resultObject = await mediator.Send(command);

            return resultObject.Match<IActionResult>(
                onSuccess: value => CreatedAtAction(nameof(GetAppointmentById), new { id = value }, value),
                onFailure: error => BadRequest(error)
            );
        }

        [Authorize(Policy = "DOCTOR_PACIENT")]
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> GetAppointmentById(Guid id)
        {
            var resultObject = await mediator.Send(new GetAppointmentByIdQuery { Id = id });
            return resultObject.Match<IActionResult>(
                onSuccess: value => Ok(value),
                onFailure: error => NotFound(error)
            );
        }
        
        [Authorize(Policy = "DOCTOR")]
        [HttpGet]   
        [EnableQuery]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        //     [SwaggerOperation(
        //     Summary = "Get all appointments with OData query support",
        //     Description = "Supports OData query options like $filter, $orderby, $top, and $skip."
        // )]
        public async Task<ActionResult<IQueryable<AppointmentDto>>> GetAllAppointments()
        {
            var resultObject = await mediator.Send(new GetAllAppointmentsQuery());
            return resultObject.Match<ActionResult>(
                onSuccess: value => Ok(value),
                onFailure: NotFound
            );
        }

        [Authorize(Policy = "DOCTOR_PACIENT")]
        [HttpPatch("Cancel")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> CancelAppointment(CancelAppointmentCommand command)
        {
            var resultObject = await mediator.Send(command);
            return resultObject.Match<IActionResult>(
                onSuccess: unit => NoContent(),
                onFailure: error => BadRequest(error)
            );
        }

        [Authorize(Policy = "DOCTOR")]
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> UpdateAppointment(Guid id, [FromBody] UpdateAppointmentCommand command)
        {
            if (id != command.Id)
            {
                return BadRequest("The ID in the URL does not match the ID in the request body.");
            }

            var resultObject = await mediator.Send(command);
            return resultObject.Match<IActionResult>(
                onSuccess: unit => NoContent(),
                onFailure: error => BadRequest(error)
            );
        }


        [Authorize(Policy = "DOCTOR")]
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> DeleteAppointment(Guid id)
        {
            var resultObject = await mediator.Send(new DeleteAppointmentCommand { AppointmentId = id });
            return resultObject.Match<IActionResult>(
                onSuccess: unit => NoContent(),
                onFailure: error => BadRequest(error)
            );
        }

        [Authorize(Policy = "PACIENT")]
        [HttpPatch("Reschedule/{id:guid}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [SwaggerOperation(Description = "User can ask to reschedule an appointment by changing the date and time of the appointment.")]
        public async Task<IActionResult> RescheduleAppointment(Guid id, [FromBody] RescheduleAppointmentCommand command)
        {
            if (id != command.AppointmentId)
            {
                return BadRequest("Route id and body id must match");
            }
            var resultObject = await mediator.Send(command);
            return resultObject.Match<IActionResult>(
                onSuccess: _ => NoContent(),
                onFailure: error => BadRequest(error)
            );
        }
    }
}
