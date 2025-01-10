using Domain.Utils;
using MediatR;

namespace Application.Use_Cases.Commands.MailCommands;

public class ForgotPasswordCommand : IRequest<Result<string>>
{
    public required string Email { get; set; }
}