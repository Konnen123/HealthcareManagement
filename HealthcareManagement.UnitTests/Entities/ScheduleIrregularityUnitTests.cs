using Domain.Entities;
using FluentAssertions;

namespace HealthcareManagement.UnitTests.Entities;

public class ScheduleIrregularityUnitTests
{
    [Fact]
    public void ScheduleIrregularity_Should_SetAndGetProperties()
    {
        // Arrange
        var scheduleIrregularity = new ScheduleIrregularity
        {
            ScheduleIrregularityId = Guid.NewGuid(),
            Date = new DateOnly(2023, 10, 1),
            StartTime = new TimeOnly(9, 0),
            EndTime = new TimeOnly(17, 0),
            Reason = "Doctor's appointment",
            DoctorId = Guid.NewGuid()
        };

        // Act & Assert
        scheduleIrregularity.ScheduleIrregularityId.Should().NotBeEmpty();
        scheduleIrregularity.Date.Should().Be(new DateOnly(2023, 10, 1));
        scheduleIrregularity.StartTime.Should().Be(new TimeOnly(9, 0));
        scheduleIrregularity.EndTime.Should().Be(new TimeOnly(17, 0));
        scheduleIrregularity.Reason.Should().Be("Doctor's appointment");
        scheduleIrregularity.DoctorId.Should().NotBeEmpty();
    }

    [Fact]
    public void ScheduleIrregularity_Should_AllowNullReason()
    {
        // Arrange
        var scheduleIrregularity = new ScheduleIrregularity
        {
            ScheduleIrregularityId = Guid.NewGuid(),
            Date = new DateOnly(2023, 10, 2),
            StartTime = new TimeOnly(10, 0),
            EndTime = new TimeOnly(18, 0),
            Reason = null,
            DoctorId = Guid.NewGuid()
        };

        // Act & Assert
        scheduleIrregularity.ScheduleIrregularityId.Should().NotBeEmpty();
        scheduleIrregularity.Date.Should().Be(new DateOnly(2023, 10, 2));
        scheduleIrregularity.StartTime.Should().Be(new TimeOnly(10, 0));
        scheduleIrregularity.EndTime.Should().Be(new TimeOnly(18, 0));
        scheduleIrregularity.Reason.Should().BeNull();
        scheduleIrregularity.DoctorId.Should().NotBeEmpty();
    }
}