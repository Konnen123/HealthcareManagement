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
        private readonly IFailedLoginAttemptsRepository _failedLoginAttemptsRepository;

        public LoginUserCommandHandler(IUsersRepository usersRepository, IPasswordHashingService passwordHashingService, 
            ITokenService tokenService, IFailedLoginAttemptsRepository failedLoginAttemptsRepository)
        {
            _usersRepository = usersRepository;
            _passwordHashingService = passwordHashingService;
            _tokenService = tokenService;
            _failedLoginAttemptsRepository = failedLoginAttemptsRepository;
        }
        public async Task<Result<string>> Handle(LoginUserCommand request, CancellationToken cancellationToken)
        {
            var userResult = await _usersRepository.GetByEmailAsync(request.Email, cancellationToken);
          
            if (!userResult.IsSuccess)
            {
                return Result<string>.Failure(AuthErrors.LoginFailed(nameof(UserAuthentication), "User not found"));
            }
            var user = userResult.Value!;
            if (_failedLoginAttemptsRepository.IsUserLockedOut(user.Id))
            {
                return Result<string>.Failure(AuthErrors.UserAccountLocked(nameof(UserAuthentication), "User account temporarely locked due to too many failed login attempts"));
            }
            if (!_passwordHashingService.VerifyPassword(request.Password, user.Password))
            {
                _failedLoginAttemptsRepository.AddFailedAttempt(user.Id);
                return Result<string>.Failure(AuthErrors.LoginFailed(nameof(UserAuthentication), "Invalid password"));
            }
            var token = _tokenService.GenerateAccessToken(user);
            _failedLoginAttemptsRepository.ResetFailedAttempts(user.Id);
            return Result<string>.Success(token);
        }
        
    }
}
