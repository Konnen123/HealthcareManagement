using Domain.Entities;
using FluentAssertions;

namespace HealthcareManagement.UnitTests.Entities;

public class ScheduleUnitTests
{
    [Fact]
    public void DailyDoctorSchedule_Should_SetAndGetProperties()
    {
        // Arrange
        var schedule = new DailyDoctorSchedule
        {
            DailyDoctorScheduleId = Guid.NewGuid(),
            DayOfWeek = DayOfWeek.Monday,
            StartingTime = new TimeOnly(9, 0),
            EndingTime = new TimeOnly(17, 0),
            SlotDurationMinutes = 30,
            DoctorId = Guid.NewGuid(),
            LocationId = Guid.NewGuid(),
            Location = new Location()
        };

        // Act & Assert
        schedule.DailyDoctorScheduleId.Should().NotBeEmpty();
        schedule.DayOfWeek.Should().Be(DayOfWeek.Monday);
        schedule.StartingTime.Should().Be(new TimeOnly(9, 0));
        schedule.EndingTime.Should().Be(new TimeOnly(17, 0));
        schedule.SlotDurationMinutes.Should().Be(30);
        schedule.DoctorId.Should().NotBeEmpty();
        schedule.LocationId.Should().NotBeEmpty();
        schedule.Location.Should().NotBeNull();
    }

    [Fact]
    public void DailyDoctorSchedule_Should_AllowNullLocation()
    {
        // Arrange
        var schedule = new DailyDoctorSchedule
        {
            DailyDoctorScheduleId = Guid.NewGuid(),
            DayOfWeek = DayOfWeek.Tuesday,
            StartingTime = new TimeOnly(10, 0),
            EndingTime = new TimeOnly(18, 0),
            SlotDurationMinutes = 45,
            DoctorId = Guid.NewGuid(),
            LocationId = Guid.NewGuid(),
            Location = null
        };

        // Act & Assert
        schedule.DailyDoctorScheduleId.Should().NotBeEmpty();
        schedule.DayOfWeek.Should().Be(DayOfWeek.Tuesday);
        schedule.StartingTime.Should().Be(new TimeOnly(10, 0));
        schedule.EndingTime.Should().Be(new TimeOnly(18, 0));
        schedule.SlotDurationMinutes.Should().Be(45);
        schedule.DoctorId.Should().NotBeEmpty();
        schedule.LocationId.Should().NotBeEmpty();
        schedule.Location.Should().BeNull();
    }
}