using Domain.Entities;
using FluentAssertions;

namespace HealthcareManagement.UnitTests.Entities;

public class AppointmentUpdateRequestUnitTests
{
    [Fact]
    public void AppointmentUpdateRequest_Should_SetAndGetProperties()
    {
        // Arrange
        var appointment = new Appointment { Id = Guid.NewGuid() };
        var appointmentUpdateRequest = new AppointmentUpdateRequest
        {
            Id = Guid.NewGuid(),
            AppointmentId = appointment.Id,
            NewDate = new DateOnly(2023, 10, 1),
            NewStartTime = new TimeOnly(9, 0),
            IsProcessed = true,
            IsAccepted = true,
            Appointment = appointment
        };

        // Act & Assert
        appointmentUpdateRequest.Id.Should().NotBeEmpty();
        appointmentUpdateRequest.AppointmentId.Should().Be(appointment.Id);
        appointmentUpdateRequest.NewDate.Should().Be(new DateOnly(2023, 10, 1));
        appointmentUpdateRequest.NewStartTime.Should().Be(new TimeOnly(9, 0));
        appointmentUpdateRequest.IsProcessed.Should().BeTrue();
        appointmentUpdateRequest.IsAccepted.Should().BeTrue();
        appointmentUpdateRequest.Appointment.Should().Be(appointment);
    }

    [Fact]
    public void AppointmentUpdateRequest_Should_AllowNullNewDateAndNewStartTime()
    {
        // Arrange
        var appointment = new Appointment { Id = Guid.NewGuid() };
        var appointmentUpdateRequest = new AppointmentUpdateRequest
        {
            Id = Guid.NewGuid(),
            AppointmentId = appointment.Id,
            NewDate = null,
            NewStartTime = null,
            IsProcessed = false,
            IsAccepted = false,
            Appointment = appointment
        };

        // Act & Assert
        appointmentUpdateRequest.Id.Should().NotBeEmpty();
        appointmentUpdateRequest.AppointmentId.Should().Be(appointment.Id);
        appointmentUpdateRequest.NewDate.Should().BeNull();
        appointmentUpdateRequest.NewStartTime.Should().BeNull();
        appointmentUpdateRequest.IsProcessed.Should().BeFalse();
        appointmentUpdateRequest.IsAccepted.Should().BeFalse();
        appointmentUpdateRequest.Appointment.Should().Be(appointment);
    }
}