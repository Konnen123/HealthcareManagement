using Domain.Entities;
using Domain.Entities.User;

namespace Domain.Services;

public interface ITokenService
{
    string GenerateAccessToken(User user);
    RefreshToken GenerateRefreshToken(User user, string? deviceInfo = null, string? ipAddress = null);
    Task<RefreshToken?> ValidateRefreshTokenAsync(string token);
}