using Application.Use_Cases.CommandHandlers.AppointmentCommandHandlers;
using Application.Use_Cases.Commands;
using AutoMapper;
using Domain.Entities;
using Domain.Repositories;
using Domain.Utils;
using MediatR;
using NSubstitute;

namespace HealthcareSystem.Application.UnitTests;

public class CancelAppointmentCommandHandlerTests
{
    private readonly IMapper _mapper;
    private readonly IAppointmentRepository _repository;
    private readonly CancelAppointmentCommandHandler _handler;

    public CancelAppointmentCommandHandlerTests()
    {
        _mapper = Substitute.For<IMapper>();
        _repository = Substitute.For<IAppointmentRepository>();
        _handler = new CancelAppointmentCommandHandler(_mapper, _repository);
    }

    [Fact]
    public async Task Handle_ShouldReturnSuccessResult_WhenAppointmentIsCanceledSuccessfully()
    {
        // Arrange
        var command = new CancelAppointmentCommand { AppointmentId = Guid.NewGuid(), PatientId = Guid.NewGuid(), CancellationReason = "Reason" };
        var appointment = new Appointment { Id = command.AppointmentId, PatientId = command.PatientId };
        var resultObject = Result<Appointment>.Success(appointment);
        var expectedResult = Result<Unit>.Success(Unit.Value);

        _repository.GetAsync(command.AppointmentId).Returns(Task.FromResult(resultObject));
        _repository.CancelAsync(command.AppointmentId, command.CancellationReason).Returns(Task.FromResult(Result<Unit>.Success(Unit.Value)));

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(expectedResult.Value, result.Value);
    }
}