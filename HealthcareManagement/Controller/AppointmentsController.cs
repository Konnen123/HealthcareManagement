using Application.Use_Cases.Commands;
using Application.Use_Cases.Commands.AppointmentCommands;
using Application.Use_Cases.Queries.AppointmentQueries;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HealthcareManagement.Controller
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class AppointmentsController : ControllerBase
    {
        private readonly IMediator mediator;

        public AppointmentsController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateAppointment([FromBody] CreateAppointmentCommand command)
        {

            var resultObject = await mediator.Send(command);
            Console.WriteLine(resultObject.Value);
            Console.WriteLine(resultObject.Error);


            return resultObject.Match<IActionResult>(
                onSuccess: value => CreatedAtAction(nameof(GetAppointmentById), new { id = value }, value),
                onFailure: error => BadRequest(error)
            );
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetAppointmentById(Guid id)
        {
            var resultObject = await mediator.Send(new GetAppointmentByIdQuery { Id = id });
            return resultObject.Match<IActionResult>(
                onSuccess: value => Ok(value),
                onFailure: error => NotFound(error)
            );
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetAllAppointments()
        {
            var resultObject = await mediator.Send(new GetAllAppointmentsQuery());
            return resultObject.Match<IActionResult>(
                onSuccess: value => Ok(value),
                onFailure: error => NotFound(error)
            );
        }


        [HttpPatch("Cancel")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CancelAppointment(CancelAppointmentCommand command)
        {
            var resultObject = await mediator.Send(command);
            return resultObject.Match<IActionResult>(
                onSuccess: unit => NoContent(),
                onFailure: error => BadRequest(error)
            );
        }


        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
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


        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> DeleteAppointment(Guid id)
        {
            var resultObject = await mediator.Send(new DeleteAppointmentCommand { AppointmentId = id });
            return resultObject.Match<IActionResult>(
                onSuccess: unit => Ok(),
                onFailure: error => BadRequest(error)
            );
        }
    }
}
