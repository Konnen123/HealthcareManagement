using Application.Use_Cases.CommandHandlers.AppointmentCommandHandlers;
using Application.Use_Cases.Commands.AppointmentCommands;
using AutoMapper;
using Domain.Entities;
using Domain.Repositories;
using Domain.Utils;
using MediatR;
using NSubstitute;

namespace HealthcareSystem.Application.UnitTests;

public class DeleteAppointmentCommandHandlerTests
{
    private readonly IMapper _mapper;
    private readonly IAppointmentRepository _repository;
    private readonly DeleteAppointmentCommandHandler _handler;

    public DeleteAppointmentCommandHandlerTests()
    {
        _mapper = Substitute.For<IMapper>();
        _repository = Substitute.For<IAppointmentRepository>();
        _handler = new DeleteAppointmentCommandHandler(_mapper, _repository);
    }

    [Fact]
    public async Task Handle_ShouldReturnSuccessResult_WhenAppointmentIsDeletedSuccessfully()
    {
        // Arrange
        var command = new DeleteAppointmentCommand { AppointmentId = Guid.NewGuid() };
        var appointment = new Appointment { Id = command.AppointmentId };
        var resultObject = Result<Appointment>.Success(appointment);
        var expectedResult = Result<Unit>.Success(Unit.Value);

        _repository.GetAsync(command.AppointmentId).Returns(Task.FromResult(resultObject));
        _repository.DeleteAsync(command.AppointmentId).Returns(Task.FromResult(Result<Unit>.Success(Unit.Value)));

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(expectedResult.Value, result.Value);
    }
}