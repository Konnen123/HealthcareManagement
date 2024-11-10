using Application.Use_Cases.Commands;
using AutoMapper;
using Domain.Errors;
using Domain.Repositories;
using Domain.Utils;
using MediatR;

namespace Application.Use_Cases.CommandHandlers.AppointmentCommandHandlers;

public class CancelAppointmentCommandHandler : IRequestHandler<CancelAppointmentCommand, Result<Unit>>
{
    private readonly IAppointmentRepository _repository;

    public CancelAppointmentCommandHandler(IAppointmentRepository repository)
    {
        _repository = repository;
    }
    
    
    public async Task<Result<Unit>> Handle(CancelAppointmentCommand command, CancellationToken cancellationToken)
    {
        var resultObject = await _repository.GetAsync(command.AppointmentId);
        if (!resultObject.IsSuccess)
        {
            return Result<Unit>.Failure(resultObject.Error!);
        }

        if (resultObject.Value!.PatientId != command.PatientId)
        {
            return Result<Unit>.Failure(AppointmentErrors.NotAllowedToCancel($"Appointment that is tried to be canceled was made by user with id {resultObject.Value.PatientId} and patient trying to cancel it has id: {command.PatientId}"));
        }
        
        if(resultObject.Value.CanceledAt.HasValue)
        {
            return Result<Unit>.Failure(AppointmentErrors.AlreadyCanceled("Appointment is already canceled"));
        }

        var secondResult = await _repository.CancelAsync(command.AppointmentId, command.CancellationReason);
        return secondResult.Match<Result<Unit>>(
            onSuccess: unit => Result<Unit>.Success(unit),
            onFailure: error => Result<Unit>.Failure(error));
    }
}