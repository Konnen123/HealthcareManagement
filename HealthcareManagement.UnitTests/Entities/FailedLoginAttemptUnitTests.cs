using Domain.Entities.User;
using FluentAssertions;

namespace HealthcareManagement.UnitTests.Entities;

public class FailedLoginAttemptUnitTests
{
    [Fact]
    public void FailedLoginAttempt_Should_SetAndGetProperties()
    {
        // Arrange
        var failedLoginAttempt = new FailedLoginAttempt
        {
            AttemptId = Guid.NewGuid(),
            FailedAttempts = 3,
            LastFailedAttemptTime = DateTime.UtcNow,
            LockoutEndTime = DateTime.UtcNow.AddMinutes(15),
            MaxFailedLoginAttempts = 5,
        };

        // Act & Assert
        failedLoginAttempt.AttemptId.Should().NotBeEmpty();
        failedLoginAttempt.FailedAttempts.Should().Be(3);
        failedLoginAttempt.LastFailedAttemptTime.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(1));
        failedLoginAttempt.LockoutEndTime.Should().BeCloseTo(DateTime.UtcNow.AddMinutes(15), TimeSpan.FromSeconds(1));
        failedLoginAttempt.MaxFailedLoginAttempts.Should().Be(5);
    }

    [Fact]
    public void FailedLoginAttempt_Should_AllowNullLockoutEndTime()
    {
        // Arrange
        var failedLoginAttempt = new FailedLoginAttempt
        {
            AttemptId = Guid.NewGuid(),
            FailedAttempts = 2,
            LastFailedAttemptTime = DateTime.UtcNow,
            LockoutEndTime = null,
            MaxFailedLoginAttempts = 5,
        };

        // Act & Assert
        failedLoginAttempt.AttemptId.Should().NotBeEmpty();
        failedLoginAttempt.FailedAttempts.Should().Be(2);
        failedLoginAttempt.LastFailedAttemptTime.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(1));
        failedLoginAttempt.LockoutEndTime.Should().BeNull();
        failedLoginAttempt.MaxFailedLoginAttempts.Should().Be(5);
    }
}