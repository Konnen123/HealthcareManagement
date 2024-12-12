using Application.Use_Cases.Commands.AuthCommands;
using Domain.Errors;
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
            var deviceInfo = Request.Headers.UserAgent.ToString(); 
            var ipAddress = HttpContext.Connection.RemoteIpAddress?.ToString();

            command.DeviceInfo = deviceInfo;
            command.IpAddress = ipAddress;
            
            var resultObject = await _mediator.Send(command);
            return resultObject.Match<IActionResult>(
                onSuccess: value => Ok(value),
                onFailure: error =>
                {
                    if(error.Code != "UserAuthentication.AccountLocked")
                    {
                        return BadRequest(error);
                    }

                    return Forbid();
                }
            );
        }
        
        [HttpPost("refresh")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Refresh([FromBody] RefreshTokenCommand command)
        {
            var deviceInfo = Request.Headers.UserAgent.ToString(); 
            var ipAddress = HttpContext.Connection.RemoteIpAddress?.ToString();
            
            command.DeviceInfo = deviceInfo;
            command.IpAddress = ipAddress;
            
            var resultObject = await _mediator.Send(command);
            return resultObject.Match<IActionResult>(
                onSuccess: value => Ok(value),
                onFailure: error => BadRequest(error)
            );
        }
        
        [HttpPost("logout")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> Logout()
        {
            var resultObject = await _mediator.Send(new LogoutCommand());
            return resultObject.Match<IActionResult>(
                onSuccess: value => NoContent(),
                onFailure: error => 
                {
                    if (error.GetType() == AuthorizationErrors.Unauthorized("","").GetType())
                    {
                        return Unauthorized(error);
                    }
                    return BadRequest(error);
                }
            );
        }
    }
}
