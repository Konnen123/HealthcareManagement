using Domain.Entities.Tokens;
using FluentAssertions;

namespace HealthcareManagement.UnitTests.Entities;

public class TokenUnitTests
{
    [Fact]
    public void RefreshToken_Should_SetAndGetProperties()
    {
        var refreshToken = new RefreshToken
        {
            RefreshTokenId = Guid.NewGuid(),
            Token = "sampleToken",
            User = null,
            IssuedAt = DateTime.UtcNow,
            ExpiresAt = DateTime.UtcNow.AddDays(1),
            DeviceInfo = "sampleDevice",
            IpAddress = "127.0.0.1",
            IsRevoked = false,
            RevokedAt = null
        };

        // Act & Assert
        refreshToken.RefreshTokenId.Should().NotBeEmpty();
        refreshToken.Token.Should().Be("sampleToken");
        refreshToken.IssuedAt.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(1));
        refreshToken.ExpiresAt.Should().BeCloseTo(DateTime.UtcNow.AddDays(1), TimeSpan.FromSeconds(1));
        refreshToken.DeviceInfo.Should().Be("sampleDevice");
        refreshToken.IpAddress.Should().Be("127.0.0.1");
        refreshToken.IsRevoked.Should().BeFalse();
        refreshToken.RevokedAt.Should().BeNull();
    }

    [Fact]
    public void ResetPasswordToken_Should_SetAndGetProperties()
    {
        // Arrange
        var resetPasswordToken = new ResetPasswordToken
        {
            ResetPasswordTokenId = Guid.NewGuid(),
            Token = "sampleToken",
            ExpiresAt = DateTime.UtcNow.AddHours(1),
            CreatedAt = DateTime.UtcNow
        };

        // Act & Assert
        resetPasswordToken.ResetPasswordTokenId.Should().NotBeEmpty();
        resetPasswordToken.Token.Should().Be("sampleToken");
        resetPasswordToken.CreatedAt.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(1));
        resetPasswordToken.ExpiresAt.Should().BeCloseTo(DateTime.UtcNow.AddHours(1), TimeSpan.FromSeconds(1));
    }

    [Fact]
    public void VerifyEmailToken_Should_SetAndGetProperties()
    {
        // Arrange
        var verifyEmailToken = new VerifyEmailToken
        {
            VerifyEmailTokenId = Guid.NewGuid(),
            Token = "sampleToken",
            CreatedAt = DateTime.UtcNow,
            ExpiresAt = DateTime.UtcNow.AddDays(1),
        };

        // Act & Assert
        verifyEmailToken.VerifyEmailTokenId.Should().NotBeEmpty();
        verifyEmailToken.Token.Should().Be("sampleToken");
        verifyEmailToken.CreatedAt.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(1));
        verifyEmailToken.ExpiresAt.Should().BeCloseTo(DateTime.UtcNow.AddDays(1), TimeSpan.FromSeconds(1));
    }
}