using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Domain.Entities;
using Domain.Entities.User;
using Domain.Repositories;
using Domain.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace Identity.Services;

public class TokenService : ITokenService
{
    private readonly IConfiguration _configuration;
    private readonly IRefreshTokenRepository _refreshTokenRepository;

    public TokenService(IConfiguration configuration, IRefreshTokenRepository refreshTokenRepository)
    {
        _configuration = configuration;
        _refreshTokenRepository = refreshTokenRepository;
    }
    
    public string GenerateAccessToken(User user)
    {
        var jwtSecret = _configuration["Jwt:Key"] ?? throw new InvalidOperationException("Unable to read JWT from config in TokenService");
        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString()),
            new Claim(ClaimTypes.Email, user.Email),
            new Claim(ClaimTypes.Role, user.Role.ToString())
        };

        var key = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(jwtSecret)
        );
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: _configuration["Jwt:Issuer"],
            audience: _configuration["Jwt:Audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddSeconds(Convert.ToDouble(_configuration["Jwt:AccessExpiry"])),
            signingCredentials: credentials
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    public RefreshToken GenerateRefreshToken(User user, string? deviceInfo = null, string? ipAddress = null)
    {
        return new RefreshToken
        {
            RefreshTokenId = Guid.NewGuid(),
            Token = GenerateSecureToken(),
            UserId = user.UserId,
            IssuedAt = DateTime.UtcNow,
            ExpiresAt = DateTime.UtcNow.AddDays(Convert.ToDouble(_configuration["Jwt:RefreshExpiry"])),
            DeviceInfo = deviceInfo,
            IpAddress = ipAddress
        };
    }

    public async Task<RefreshToken?> ValidateRefreshTokenAsync(string token)
    {
        var storedRefreshTokenResult = await _refreshTokenRepository.GetRefreshTokenAsync(token);
        if (!storedRefreshTokenResult.IsSuccess || storedRefreshTokenResult.Value is null)
            return null;
        var storedRefreshToken = storedRefreshTokenResult.Value!;
        
        if (storedRefreshToken.ExpiresAt < DateTime.UtcNow)
            return null;
        
        return storedRefreshToken;
    }
    
    private static string GenerateSecureToken()
    {
        return Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));
    }
}