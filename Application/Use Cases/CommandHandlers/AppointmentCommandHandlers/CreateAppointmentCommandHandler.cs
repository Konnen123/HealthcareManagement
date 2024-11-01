using Application.Use_Cases.Commands;
using AutoMapper;
using Domain.Entities;
using Domain.Repositories;
using Domain.Utils;
using MediatR;

namespace Application.Use_Cases.CommandHandlers.AppointmentCommandHandlers;

public class CreateAppointmentCommandHandler : IRequestHandler<CreateAppointmentCommand, Result<Guid>>
{
    private readonly IMapper _mapper;
    private readonly IAppointmentRepository _repository;

    public CreateAppointmentCommandHandler(IMapper mapper, IAppointmentRepository repository)
    {
        _mapper = mapper;
        _repository = repository;
    }
    
    
    public async Task<Result<Guid>> Handle(CreateAppointmentCommand request, CancellationToken cancellationToken)
    {
        var appointment = _mapper.Map<Appointment>(request);
        var resultObject = await _repository.AddAsync(appointment);
        return resultObject.Match<Result<Guid>>(
            onSuccess: value => Result<Guid>.Success(value),
            onFailure: error => Result<Guid>.Failure(error));
    }
}