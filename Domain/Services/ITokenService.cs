using Domain.Entities;
using Domain.Entities.User;

namespace Domain.Services;

public interface ITokenService
{
    string GenerateAccessToken(UserAuthentication user);
    RefreshToken GenerateRefreshToken(UserAuthentication user, string? deviceInfo = null, string? ipAddress = null);
    Task<RefreshToken?> ValidateRefreshTokenAsync(string token);
}