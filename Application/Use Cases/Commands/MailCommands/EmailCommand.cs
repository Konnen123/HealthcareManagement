using Domain.Utils;
using MediatR;

namespace Application.Use_Cases.Commands.MailCommands;

public abstract class EmailCommand : IRequest<Result<string>>
{
    public required string Email { get; set; }
}