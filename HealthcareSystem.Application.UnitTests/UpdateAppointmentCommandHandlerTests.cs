using Application.Use_Cases.CommandHandlers.AppointmentCommandHandlers;
using Application.Use_Cases.Commands.AppointmentCommands;
using AutoMapper;
using Domain.Entities;
using Domain.Repositories;
using Domain.Utils;
using MediatR;
using NSubstitute;

namespace HealthcareSystem.Application.UnitTests;

public class UpdateAppointmentCommandHandlerTests
{
    private readonly IMapper _mapper;
    private readonly IAppointmentRepository _repository;
    private readonly UpdateAppointmentCommandHandler _handler;

    public UpdateAppointmentCommandHandlerTests()
    {
        _mapper = Substitute.For<IMapper>();
        _repository = Substitute.For<IAppointmentRepository>();
        _handler = new UpdateAppointmentCommandHandler(_mapper, _repository);
    }

    [Fact]
    public async Task Handle_ShouldReturnSuccessResult_WhenAppointmentIsUpdatedSuccessfully()
    {
        // Arrange
        var command = new UpdateAppointmentCommand { Id = Guid.NewGuid(), PatientId = Guid.NewGuid(), StartTime = DateTime.Now.AddDays(1) };
        var appointment = new Appointment { Id = command.Id, PatientId = command.PatientId };
        var resultObject = Result<Unit>.Success(Unit.Value);

        _mapper.Map<Appointment>(command).Returns(appointment);
        _repository.UpdateAsync(appointment).Returns(Task.FromResult(resultObject));

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(resultObject.Value, result.Value);
    }
}