using Application.Use_Cases.Commands.AuthCommands;
using Domain.Repositories;
using Domain.Utils;
using MediatR;

namespace Application.Use_Cases.CommandHandlers.AuthCommandHandlers
{
    public class LoginUserCommandHandler : IRequestHandler<LoginUserCommand, Result<string>>
    {
        private readonly IUsersRepository _usersRepository;
        public LoginUserCommandHandler(IUsersRepository usersRepository)
        {
            _usersRepository = usersRepository;
        }
        public Task<Result<string>> Handle(LoginUserCommand request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
