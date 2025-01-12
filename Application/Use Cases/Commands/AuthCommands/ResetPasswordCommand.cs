using Domain.Utils;
using MediatR;

namespace Application.Use_Cases.Commands.AuthCommands;

public class ResetPasswordCommand : IRequest<Result<string>>
{
    public string Password { get; set; } = null!;
    public string Token { get; set; } = null!;
}