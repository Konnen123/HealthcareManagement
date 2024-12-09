using Application.Use_Cases.Commands.AuthCommands;
using Application.Use_Cases.Responses;
using Domain.Entities;
using Domain.Entities.User;
using Domain.Errors;
using Domain.Repositories;
using Domain.Services;
using Domain.Utils;
using MediatR;

namespace Application.Use_Cases.CommandHandlers.AuthCommandHandlers
{
    public class LoginUserCommandHandler : IRequestHandler<LoginUserCommand, Result<TokenResponse>>
    {
        private readonly IUsersRepository _usersRepository;
        private readonly IPasswordHashingService _passwordHashingService;
        private readonly ITokenService _tokenService;
        private readonly IFailedLoginAttemptsRepository _failedLoginAttemptsRepository;
        private readonly IRefreshTokenRepository _refreshTokenRepository;

        public LoginUserCommandHandler(IUsersRepository usersRepository, IPasswordHashingService passwordHashingService, 
            ITokenService tokenService, IFailedLoginAttemptsRepository failedLoginAttemptsRepository, IRefreshTokenRepository refreshTokenRepository)
        {
            _usersRepository = usersRepository;
            _passwordHashingService = passwordHashingService;
            _tokenService = tokenService;
            _failedLoginAttemptsRepository = failedLoginAttemptsRepository;
            _refreshTokenRepository = refreshTokenRepository;
        }
        public async Task<Result<TokenResponse>> Handle(LoginUserCommand request, CancellationToken cancellationToken)
        {
            var userResult = await _usersRepository.GetByEmailAsync(request.Email, cancellationToken);
          
            if (!userResult.IsSuccess)
            {
                return Result<TokenResponse>.Failure(AuthErrors.LoginFailed(nameof(UserAuthentication), "User not found"));
            }
            var user = userResult.Value!;
            if (_failedLoginAttemptsRepository.IsUserLockedOut(user.UserId))
            {
                return Result<TokenResponse>.Failure(AuthErrors.UserAccountLocked(nameof(UserAuthentication), "User account temporarely locked due to too many failed login attempts"));
            }
            if (!_passwordHashingService.VerifyPassword(request.Password, user.Password))
            {
                _failedLoginAttemptsRepository.AddFailedAttempt(user.UserId);
                return Result<TokenResponse>.Failure(AuthErrors.LoginFailed(nameof(UserAuthentication), "Invalid password"));
            }

            var existingTokensResult = await _refreshTokenRepository.GetByUserIdAsync(user.UserId);
            if (!existingTokensResult.IsSuccess)
            {
                return Result<TokenResponse>.Failure(EntityErrors.GetFailed(nameof(RefreshToken), "An unexpected error occurred while retrieving refresh tokens for user."));
            }
            
            foreach (var refreshTokenEntry in existingTokensResult.Value!)
            {
                refreshTokenEntry.IsRevoked = true;
                refreshTokenEntry.RevokedAt = DateTime.UtcNow;
                await _refreshTokenRepository.UpdateAsync(refreshTokenEntry);
            }
            
            var token = _tokenService.GenerateAccessToken(user);
            _failedLoginAttemptsRepository.ResetFailedAttempts(user.UserId);
            
            var refreshToken = _tokenService.GenerateRefreshToken(user, request.DeviceInfo, request.IpAddress);
            refreshToken.UserAuthentication = user; 
        
            await _refreshTokenRepository.AddRefreshTokenAsync(refreshToken);
            
            return Result<TokenResponse>.Success(new TokenResponse(token, refreshToken.Token));
        }
    }
}
