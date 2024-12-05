using Application.Use_Cases.Commands.AuthCommands;
using Domain.Repositories;
using Domain.Utils;
using MediatR;

namespace Application.Use_Cases.CommandHandlers.AuthCommandHandlers
{
    public class RegisterUserCommandHandler : IRequestHandler<RegisterUserCommand, Result<Guid>>
    {
        private readonly IUsersRepository _usersRepository;

        public RegisterUserCommandHandler(IUsersRepository userRepository)
        {
            _usersRepository = userRepository;
        }
        public Task<Result<Guid>> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
