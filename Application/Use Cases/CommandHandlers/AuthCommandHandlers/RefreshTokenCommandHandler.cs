using Application.Use_Cases.Commands.AuthCommands;
using Application.Use_Cases.Responses;
using Domain.Entities;
using Domain.Entities.User;
using Domain.Errors;
using Domain.Repositories;
using Domain.Services;
using Domain.Utils;
using MediatR;

namespace Application.Use_Cases.CommandHandlers.AuthCommandHandlers;

public class RefreshTokenCommandHandler : IRequestHandler<RefreshTokenCommand, Result<TokenResponse>>
{
    private readonly IUsersRepository _usersRepository;
    private readonly ITokenService _tokenService;
    private readonly IRefreshTokenRepository _refreshTokenRepository;

    public RefreshTokenCommandHandler(IUsersRepository usersRepository, ITokenService tokenService, IRefreshTokenRepository refreshTokenRepository)
    {
        _usersRepository = usersRepository;
        _tokenService = tokenService;
        _refreshTokenRepository = refreshTokenRepository;
    }
    public async Task<Result<TokenResponse>> Handle(RefreshTokenCommand request, CancellationToken cancellationToken)
    {
        var refreshToken = await _tokenService.ValidateRefreshTokenAsync(request.RefreshToken);
        if (refreshToken == null || refreshToken.IsRevoked || refreshToken.ExpiresAt < DateTime.UtcNow)
            return Result<TokenResponse>.Failure(EntityErrors.GetFailed(nameof(RefreshToken), "Invalid or expired refresh token"));
        
        //deocamdata n am gasit alta varianta decat sa iau squad memberul din refresh token

        var user = await _usersRepository.GetByIdAsync(refreshToken.UserId, cancellationToken);
        if (!user.IsSuccess)
            return Result<TokenResponse>.Failure(EntityErrors.GetFailed(nameof(UserAuthentication), "User not found"));

        refreshToken.IsRevoked = true;
        refreshToken.RevokedAt = DateTime.UtcNow;
        
        var newAccessToken = _tokenService.GenerateAccessToken(user.Value!);
        var newRefreshToken = _tokenService.GenerateRefreshToken(user.Value!);
        
        await _refreshTokenRepository.AddRefreshTokenAsync(newRefreshToken);
        await _refreshTokenRepository.UpdateAsync(refreshToken);
        
        return Result<TokenResponse>.Success(new TokenResponse(newAccessToken, newRefreshToken.Token));
    }
}