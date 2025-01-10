using Domain.Entities.User;

namespace Domain.Services;

public interface ITokenService
{
    string GenerateAccessToken(User user);
    RefreshToken GenerateRefreshToken(User user, string? deviceInfo = null, string? ipAddress = null);
    ResetPasswordToken GenerateResetPasswordToken(User user);
    Task<RefreshToken?> ValidateRefreshTokenAsync(string token);
    Task<ResetPasswordToken?> ValidateResetPasswordTokenAsync(string token);
}