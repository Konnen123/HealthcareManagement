using Application.Use_Cases.Commands.AuthCommands;
using Domain.Utils;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace HealthcareManagement.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IMediator _mediator;
        public AuthController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("Register")]
        public async Task<IActionResult> Register(RegisterUserCommand command)
        {
            var resultObject = await _mediator.Send(command);
            return resultObject.Match<IActionResult>(
                onSuccess: value => Ok(value),
                onFailure: error => BadRequest(error)
            );
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login(LoginUserCommand command)
        {
            var resultObject = await _mediator.Send(command);
            return resultObject.Match<IActionResult>(
                onSuccess: value => Ok(new { JwtToken = value }),
                onFailure: error => BadRequest(error)
            );
        }
    }
}
