using Domain.Utils;
using MediatR;

namespace Application.Use_Cases.Commands.AuthCommands
{
    public class RegisterUserCommand : IRequest<Result<Guid>>
    {
        public required string FirstName { get; set; }

        public required string LastName { get; set; }

        public required string Email { get; set; }
        public required string Password { get; set; }

        public required string PhoneNumber { get; set; }
        public DateOnly DateOfBirth { get; set; }
        public Roles Role { get; set; }
    }
}

