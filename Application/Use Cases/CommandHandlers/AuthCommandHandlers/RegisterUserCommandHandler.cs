using Application.Use_Cases.Commands.AuthCommands;
using AutoMapper;
using Domain.Entities.User;
using Domain.Repositories;
using Domain.Services;
using Domain.Utils;
using MediatR;

namespace Application.Use_Cases.CommandHandlers.AuthCommandHandlers
{
    public class RegisterUserCommandHandler : IRequestHandler<RegisterUserCommand, Result<Guid>>
    {
        private readonly IUsersRepository _usersRepository;
        private readonly IMapper _mapper;
        private readonly IPasswordHashingService _passwordHashingService;

        public RegisterUserCommandHandler(IUsersRepository userRepository, IMapper mapper, IPasswordHashingService passwordHashingService)
        {
            _usersRepository = userRepository;
            _mapper = mapper;
            _passwordHashingService = passwordHashingService;
        }
        public async Task<Result<Guid>> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
        {
            var hashedPassword = _passwordHashingService.HashPassword(request.Password);
            var user = _mapper.Map<UserAuthentication>(request);
            user.Password = hashedPassword;
            
            var resultObject = await _usersRepository.Register(user, cancellationToken);
            return resultObject.Match<Result<Guid>>(
                onSuccess: value => Result<Guid>.Success(value),
                onFailure: error => Result<Guid>.Failure(error));
        }
    }
}
