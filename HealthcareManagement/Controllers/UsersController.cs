using Application.Use_Cases.Queries.UserQueries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HealthcareManagement.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IMediator mediator;

        public UsersController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [Authorize(Policy = "DOCTOR_PATIENT")]
        [HttpGet("Doctors")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]

        public async Task<IActionResult> GetAllDoctors()
        {
            var result = await mediator.Send(new GetAllDoctorsQuery());
            return result.Match<IActionResult>(
                onSuccess: value => Ok(value),
                onFailure: error => BadRequest(error)
            );
        }
    }
}
