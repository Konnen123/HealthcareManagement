using Application.Use_Cases.Commands.AppointmentCommands;
using AutoMapper;
using Domain.Entities;
using Domain.Errors;
using Domain.Repositories;
using Domain.Utils;
using MediatR;

namespace Application.Use_Cases.CommandHandlers.AppointmentCommandHandlers;

public class RescheduleAppointmentCommandHandler : IRequestHandler<RescheduleAppointmentCommand, Result<Unit>>
{
    private readonly IMapper _mapper;
    private readonly IAppointmentRepository _appointmentRepository;
    private readonly IRescheduledAppointmentsRepository _rescheduledAppointmentsRepository;

    public RescheduleAppointmentCommandHandler(IMapper mapper, IAppointmentRepository appointmentRepository, IRescheduledAppointmentsRepository rescheduledAppointmentsRepository)
    {
        _mapper = mapper;
        _appointmentRepository = appointmentRepository;
        _rescheduledAppointmentsRepository = rescheduledAppointmentsRepository;
    }
    
    public async Task<Result<Unit>> Handle(RescheduleAppointmentCommand command, CancellationToken cancellationToken)
    {
        var resultObject = await _appointmentRepository.GetAsync(command.AppointmentId);
        if (!resultObject.IsSuccess)
        {
            return Result<Unit>.Failure(resultObject.Error!);
        }    
        if (resultObject.Value!.PatientId != command.PatientId)
        {
            return Result<Unit>.Failure(RescheduledAppointmentsErrors.NotAllowedToReschedule($"Appointment that is tried to be rescheduled was made by user with id {resultObject.Value.PatientId} and patient trying to cancel it has id: {command.PatientId}"));
        }
        
        //depinde de logica, daca e anulat un appointment se poate reporgrama?
        // if(resultObject.Value.CanceledAt.HasValue)
        // {
        //     return Result<Unit>.Failure(AppointmentErrors.AlreadyCanceled("Appointment is already canceled"));
        // }

        //aici as avea 2 variante:
        // 1. cand el face un request, sterg toate requesturile anterioare pt acea programare
        // 2. cand face request, daca cumva mai exista unul pt acea programare, nu il las.

        var potentialExistingScheduleResult = await _rescheduledAppointmentsRepository.GetAsync(command.AppointmentId);
        if(potentialExistingScheduleResult.Value != null)
        {
            return Result<Unit>.Failure(RescheduledAppointmentsErrors.AlreadyRescheduled("Appointment is already waiting to be rescheduled"));
        }
        
        var rescheduledAppointment = _mapper.Map<AppointmentUpdateRequest>(command);
        var resultToReschedule = await _rescheduledAppointmentsRepository.AddAsync(rescheduledAppointment);
        if (!resultToReschedule.IsSuccess)
        {
            return Result<Unit>.Failure(resultToReschedule.Error!);
        }
        
        return Result<Unit>.Success(default);
    }
}