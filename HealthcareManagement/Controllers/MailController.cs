using Application.Use_Cases.Commands.MailCommands;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace HealthcareManagement.Controllers;

[Route("api/v1/[controller]")]
[ApiController]
public class MailController : ControllerBase
{
    private readonly IMediator _mediator;

    public MailController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("forgot-password")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> ForgotPassword(ForgotPasswordCommand forgotPasswordCommand)
    {
        var resultObject = await _mediator.Send(forgotPasswordCommand);
        return resultObject.Match<IActionResult>(
            onSuccess: value => Ok(value),
            onFailure: error => BadRequest(error)
        );
    }
    
    [HttpPost(("verify-email"))]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> VerifyEmail(VerifyEmailCommand verifyEmailCommand)
    {
        var resultObject = await _mediator.Send(verifyEmailCommand);
        return resultObject.Match<IActionResult>(
            onSuccess: value => Ok(value),
            onFailure: error => BadRequest(error)
        );
    }
}