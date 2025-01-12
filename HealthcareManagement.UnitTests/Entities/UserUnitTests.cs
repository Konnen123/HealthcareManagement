using Domain.Entities.Tokens;
using Domain.Entities.User;
using Domain.Utils;
using FluentAssertions;

namespace HealthcareManagement.UnitTests.Entities;

public class UserUnitTests
{
    private class TestUser : User
    {
        // Concrete implementation for testing
    }

    [Fact]
    public void User_Should_SetAndGetProperties()
    {
        // Arrange
        var user = new TestUser
        {
            UserId = Guid.NewGuid(),
            FirstName = "John",
            LastName = "Doe",
            Email = "john.doe@example.com",
            Password = "password123",
            PhoneNumber = "1234567890",
            DateOfBirth = new DateOnly(1990, 1, 1),
            CreatedAt = DateOnly.FromDateTime(DateTime.UtcNow),
            IsEnabled = true,
            Role = Roles.DOCTOR,
            RefreshTokens = new List<RefreshToken>(),
            ResetPasswordToken = new ResetPasswordToken(),
            VerifyEmailToken = new VerifyEmailToken(),
            HasVerifiedEmail = true
        };

        // Act & Assert
        user.UserId.Should().NotBeEmpty();
        user.FirstName.Should().Be("John");
        user.LastName.Should().Be("Doe");
        user.Email.Should().Be("john.doe@example.com");
        user.Password.Should().Be("password123");
        user.PhoneNumber.Should().Be("1234567890");
        user.DateOfBirth.Should().Be(new DateOnly(1990, 1, 1));
        user.CreatedAt.Should().Be(DateOnly.FromDateTime(DateTime.UtcNow));
        user.IsEnabled.Should().BeTrue();
        user.Role.Should().Be(Roles.DOCTOR);
        user.RefreshTokens.Should().NotBeNull();
        user.ResetPasswordToken.Should().NotBeNull();
        user.VerifyEmailToken.Should().NotBeNull();
        user.HasVerifiedEmail.Should().BeTrue();
    }
}