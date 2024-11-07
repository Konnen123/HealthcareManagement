using Application.Use_Cases.CommandHandlers.AppointmentCommandHandlers;
using Application.Use_Cases.Commands;
using AutoMapper;
using Domain.Entities;
using Domain.Repositories;
using Domain.Utils;
using NSubstitute;

namespace HealthcareSystem.Application.UnitTests;

public class CreateAppointmentCommandHandlerTests
{
    private readonly IMapper _mapper;
    private readonly IAppointmentRepository _repository;
    private readonly CreateAppointmentCommandHandler _handler;

    public CreateAppointmentCommandHandlerTests()
    {
        _mapper = Substitute.For<IMapper>();
        _repository = Substitute.For<IAppointmentRepository>();
        _handler = new CreateAppointmentCommandHandler(_mapper, _repository);
    }
    
    [Fact]
    public async Task Handle_ShouldReturnSuccessResult_WhenAppointmentIsAddedSuccessfully()
    {
        // Arrange
        var command = new CreateAppointmentCommand {  };
        var appointment = new Appointment {  };
        var expectedResult = Result<Guid>.Success(Guid.NewGuid());

        _mapper.Map<Appointment>(command).Returns(appointment);
        _repository.AddAsync(appointment).Returns(Task.FromResult(expectedResult));

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(expectedResult.Value, result.Value);
    }
}