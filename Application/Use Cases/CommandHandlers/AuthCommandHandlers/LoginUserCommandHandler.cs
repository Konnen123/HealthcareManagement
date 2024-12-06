using Application.Use_Cases.Commands.AuthCommands;
using Domain.Entities.User;
using Domain.Errors;
using Domain.Repositories;
using Domain.Services;
using Domain.Utils;
using MediatR;

namespace Application.Use_Cases.CommandHandlers.AuthCommandHandlers
{
    public class LoginUserCommandHandler : IRequestHandler<LoginUserCommand, Result<string>>
    {
        private readonly IUsersRepository _usersRepository;
        private readonly IPasswordHashingService _passwordHashingService;
        private readonly ITokenService _tokenService;

        public LoginUserCommandHandler(IUsersRepository usersRepository, IPasswordHashingService passwordHashingService, ITokenService tokenService)
        {
            _usersRepository = usersRepository;
            _passwordHashingService = passwordHashingService;
            _tokenService = tokenService;
        }
        public async Task<Result<string>> Handle(LoginUserCommand request, CancellationToken cancellationToken)
        {
            var userResult = await _usersRepository.GetByEmailAsync(request.Email, cancellationToken);
            if (!userResult.IsSuccess)
            {
                return Result<string>.Failure(AuthErrors.LoginFailed(nameof(UserAuthentication), "User not found"));
            }
            var user = userResult.Value!;
            if (!_passwordHashingService.VerifyPassword(request.Password, user.Password))
            {
                return Result<string>.Failure(AuthErrors.LoginFailed(nameof(UserAuthentication), "Invalid password"));
            }
            var token = _tokenService.GenerateAccessToken(user);
            return Result<string>.Success(token);
        }
        
    }
}
