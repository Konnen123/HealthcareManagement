using Domain.Utils;
using MediatR;

namespace Application.Use_Cases.Commands.AuthCommands
{
    public class LoginUserCommand : IRequest<Result<string>>
    {
        public required string Email { get; set; }
        public required string Password { get; set; }
    }
}
