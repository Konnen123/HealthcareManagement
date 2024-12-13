using Application.Use_Cases.Commands;
using AutoMapper;
using Domain.Entities;
using Domain.Entities.User;
using Domain.Errors;
using Domain.Repositories;
using Domain.Utils;
using MediatR;

namespace Application.Use_Cases.CommandHandlers.AppointmentCommandHandlers;

public class CreateAppointmentCommandHandler : IRequestHandler<CreateAppointmentCommand, Result<Guid>>
{
    private readonly IMapper mapper;
    private readonly IAppointmentRepository repository;
    private readonly IDoctorsRepository doctorsRepository;
    private readonly IPatientsRepository patientsRepository;

    public CreateAppointmentCommandHandler(IMapper mapper,
        IAppointmentRepository repository, 
        IDoctorsRepository doctorsRepository, 
        IPatientsRepository patientsRepository)
    {
        this.mapper = mapper;
        this.repository = repository;
        this.doctorsRepository = doctorsRepository;
        this.patientsRepository = patientsRepository;
    }


    public async Task<Result<Guid>> Handle(CreateAppointmentCommand request, CancellationToken cancellationToken)
    {
        var doctorResult = await doctorsRepository.GetByIdAsync(request.DoctorId, cancellationToken);
        if (!doctorResult.IsSuccess)
        {
            return Result<Guid>.Failure(EntityErrors.GetFailed(nameof(Doctor), "Doctor not found"));
        }

        var patientResult = await patientsRepository.GetByIdAsync(request.PatientId, cancellationToken);
        if (!patientResult.IsSuccess)
        {
            return Result<Guid>.Failure(EntityErrors.GetFailed(nameof(Patient), "Patient not found"));
        }

        var appointment = mapper.Map<Appointment>(request);
        var resultObject = await repository.AddAsync(appointment);
        return resultObject.Match<Result<Guid>>(
            onSuccess: value => Result<Guid>.Success(value),
            onFailure: error => Result<Guid>.Failure(error));
    }
}