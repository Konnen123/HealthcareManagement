using Domain.Utils;
using MediatR;

namespace Application.Use_Cases.Commands.AuthCommands;

public class EmailTokenCommand : IRequest<Result<string>>
{
    public string Token { get; set; }
}