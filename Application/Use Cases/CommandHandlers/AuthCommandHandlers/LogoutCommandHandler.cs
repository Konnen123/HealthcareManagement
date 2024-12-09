using System.Security.Claims;
using Application.Use_Cases.Commands.AuthCommands;
using Domain.Entities.User;
using Domain.Errors;
using Domain.Repositories;
using Domain.Utils;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace Application.Use_Cases.CommandHandlers.AuthCommandHandlers;

public class LogoutCommandHandler : IRequestHandler<LogoutCommand, Result<Unit>>
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IRefreshTokenRepository _refreshTokenRepository;

    public LogoutCommandHandler(IHttpContextAccessor httpContextAccessor, IRefreshTokenRepository refreshTokenRepository)
    {
        _httpContextAccessor = httpContextAccessor;
        _refreshTokenRepository = refreshTokenRepository;
    }
    public async Task<Result<Unit>> Handle(LogoutCommand request, CancellationToken cancellationToken)
    {
        var userIdClaim = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier);
        Console.WriteLine(userIdClaim);
        if (userIdClaim == null)
        {
            return Result<Unit>.Failure(AuthorizationErrors.Unauthorized(nameof(UserAuthentication), "User is not logged in"));
        }
        
        var userId = Guid.Parse(userIdClaim.Value);
        var refreshTokensResult = await _refreshTokenRepository.GetByUserIdAsync(userId);
        if(!refreshTokensResult.IsSuccess)
        {
            return Result<Unit>.Failure(refreshTokensResult.Error!);
        }

        foreach (var refreshTokenEntry in refreshTokensResult.Value!)
        {
            refreshTokenEntry.IsRevoked = true;
            refreshTokenEntry.RevokedAt = DateTime.UtcNow;
            await _refreshTokenRepository.UpdateAsync(refreshTokenEntry);
        }
        
        return Result<Unit>.Success(Unit.Value);
    }
}