using Domain.Entities;
using FluentAssertions;

namespace HealthcareManagement.UnitTests.Entities;

public class LocationUnitTests
{
    [Fact]
    public void Location_Should_SetAndGetProperties()
    {
        // Arrange
        var location = new Location
        {
            LocationId = Guid.NewGuid(),
            RoomNo = 101,
            Floor = 1,
            Indications = "Near the entrance",
            DoctorSchedules = new List<DailyDoctorSchedule>()
        };

        // Act & Assert
        location.LocationId.Should().NotBeEmpty();
        location.RoomNo.Should().Be(101);
        location.Floor.Should().Be(1);
        location.Indications.Should().Be("Near the entrance");
        location.DoctorSchedules.Should().NotBeNull();
    }

    [Fact]
    public void Location_Should_AllowNullIndications()
    {
        // Arrange
        var location = new Location
        {
            LocationId = Guid.NewGuid(),
            RoomNo = 102,
            Floor = 2,
            Indications = null,
            DoctorSchedules = new List<DailyDoctorSchedule>()
        };

        // Act & Assert
        location.LocationId.Should().NotBeEmpty();
        location.RoomNo.Should().Be(102);
        location.Floor.Should().Be(2);
        location.Indications.Should().BeNull();
        location.DoctorSchedules.Should().NotBeNull();
    }
}