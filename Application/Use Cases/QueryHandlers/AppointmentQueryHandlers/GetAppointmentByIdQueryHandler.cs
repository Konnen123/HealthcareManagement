using Application.DTOs;
using Application.DTOs.UserDto;
using Application.Use_Cases.Queries.AppointmentQueries;
using AutoMapper;
using Domain.Entities;
using Domain.Entities.User;
using Domain.Repositories;
using Domain.Utils;
using MediatR;

namespace Application.Use_Cases.QueryHandlers.AppointmentQueryHandlers;

public class GetAppointmentByIdQueryHandler : IRequestHandler<GetAppointmentByIdQuery, Result<AppointmentDto>>
{
    private readonly IMapper mapper;
    private readonly IAppointmentRepository repository;
    private readonly IDoctorsRepository doctorsRepository;
    private readonly IPatientsRepository patientRepository;

    public GetAppointmentByIdQueryHandler(IMapper mapper,
        IAppointmentRepository repository,
        IDoctorsRepository doctorsRepository,
        IPatientsRepository patientRepository)
    {
        this.mapper = mapper;
        this.repository = repository;
        this.doctorsRepository = doctorsRepository;
        this.patientRepository = patientRepository;
    }

    public async Task<Result<AppointmentDto>> Handle(GetAppointmentByIdQuery request, CancellationToken cancellationToken)
    {
        var result = await repository.GetAsync(request.Id);
        if (!result.IsSuccess)
        {
            return Result<AppointmentDto>.Failure(result.Error);
        }

        Appointment appointment = result.Value!;
        Result<Patient> patientResult = await patientRepository.GetByIdAsync(appointment.PatientId, cancellationToken);
        Result<Doctor> doctorResult = await doctorsRepository.GetByIdAsync(appointment.DoctorId, cancellationToken);

        if(!patientResult.IsSuccess || !doctorResult.IsSuccess)
        {
            return Result<AppointmentDto>.Failure(new Error("MappingError", "Failed to map patient or doctor."));
        }

        AppointmentDto appointmentDto = mapper.Map<AppointmentDto>(appointment);
        appointmentDto.Patient = mapper.Map<PatientDto>(patientResult.Value);
        appointmentDto.Doctor = mapper.Map<DoctorDto>(doctorResult.Value);

        return result.Match(
            onSuccess: value => Result<AppointmentDto>.Success(appointmentDto),
            onFailure: error => Result<AppointmentDto>.Failure(error)
        );
    }
}