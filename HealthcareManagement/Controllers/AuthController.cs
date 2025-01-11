using Application.Use_Cases.Commands.AuthCommands;
using Application.Use_Cases.Queries.UserQueries;
using Domain.Errors;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace HealthcareManagement.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;

        public AuthController(IMediator mediator, IHttpClientFactory httpClientFactory, IConfiguration configuration)
        {
            _mediator = mediator;
            _httpClientFactory = httpClientFactory;
            _configuration = configuration;
        }

        [HttpPost("Register")]
        public async Task<IActionResult> Register(RegisterUserCommand command)
        {
            var resultObject = await _mediator.Send(command);
            return resultObject.Match<IActionResult>(
                onSuccess: value =>
                {
     
                        var client = _httpClientFactory.CreateClient();
                        var server = _configuration["Server"];
                        var request = new HttpRequestMessage(HttpMethod.Post, $"{server}/api/v1/Mail/verify-email")
                        {
                            Content = new StringContent(
                                System.Text.Json.JsonSerializer.Serialize(new { Email = command.Email }),
                                System.Text.Encoding.UTF8,
                                "application/json"
                            )
                        };

                        var response = client.Send(request);

                        if (!response.IsSuccessStatusCode)
                        {
                            throw new Exception("Failed to send verification email.");
                        }
                    
                  
                    return Ok(value);
                },
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
                    if(error.GetType() == AuthorizationErrors.Unauthorized("","").GetType())
                    {
                        return Unauthorized(error);
                    }

                    return BadRequest(error);
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

        [HttpPut("reset-password")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordCommand command)
        {
            var resultObject = await _mediator.Send(command);
            return resultObject.Match<IActionResult>(
                onSuccess: value => Ok(value),
                onFailure: error => 
                {
                    if (error.GetType() == AuthorizationErrors.Unauthorized("","").GetType())
                    {
                        return Unauthorized(resultObject.Value);
                    }
                    return BadRequest(error);
                }
            );
        }
        
        [HttpGet("verify-email")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> VerifyEmail([FromQuery] string token)
        {
            string decodedToken = Uri.UnescapeDataString(token);
            var resultObject = await _mediator.Send(new VerifyEmailQuery{Token = decodedToken});
            return resultObject.Match<IActionResult>(
                onSuccess: value => Redirect($"http://localhost:4200/{value}"),
                onFailure: error =>
                {
                    return Redirect($"http://localhost:4200/{error.Description}");
                }
            );
        }

       
    }
}
