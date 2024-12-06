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
            return Ok(Result<string>.Success("Hello register endpoint"));
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login(LoginUserCommand command)
        {
            return Ok(Result<string>.Success("Hello login endpoint"));
        }
    }
}
