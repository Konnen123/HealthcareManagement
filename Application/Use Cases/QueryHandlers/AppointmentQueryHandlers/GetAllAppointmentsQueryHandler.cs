﻿using Application.DTOs;
using Application.DTOs.UserDto;
using Application.Use_Cases.Queries.AppointmentQueries;
using AutoMapper;
using Domain.Entities;
using Domain.Entities.User;
using Domain.Repositories;
using Domain.Utils;
using MediatR;

namespace Application.Use_Cases.QueryHandlers.AppointmentQueryHandlers;

public class GetAllAppointmentsQueryHandler : IRequestHandler<GetAllAppointmentsQuery, Result<List<AppointmentDto>>>
{
    private readonly IMapper mapper;
    private readonly IAppointmentRepository repository;
    private readonly IDoctorsRepository doctorsRepository;
    private readonly IPatientsRepository patientRepository;

    public GetAllAppointmentsQueryHandler(IMapper mapper,
        IAppointmentRepository repository,
        IDoctorsRepository doctorsRepository,
        IPatientsRepository patientRepository)
    {
        this.mapper = mapper;
        this.repository = repository;
        this.doctorsRepository = doctorsRepository;
        this.patientRepository = patientRepository;
    }

    public async Task<Result<List<AppointmentDto>>> Handle(GetAllAppointmentsQuery request, CancellationToken cancellationToken)
    {
        var result = await repository.GetAllAsync();
        if (!result.IsSuccess)
        {
            return Result<List<AppointmentDto>>.Failure(result.Error);
        }

        List<Appointment> appointments = result.Value.ToList();
        List<AppointmentDto> appointmentDtos = new List<AppointmentDto>();

        foreach (var appointment in appointments)
        {
            Result<Patient> patientResult = await patientRepository.GetByIdAsync(appointment.PatientId, cancellationToken);
            Result<Doctor> doctorResult = await doctorsRepository.GetByIdAsync(appointment.DoctorId, cancellationToken);

            if (!patientResult.IsSuccess || !doctorResult.IsSuccess)
            {
                return Result<List<AppointmentDto>>.Failure(new Error("MappingError", "Failed to map patient or doctor."));
            }

            AppointmentDto appointmentDto = mapper.Map<AppointmentDto>(appointment);
            appointmentDto.Patient = mapper.Map<PatientDto>(patientResult.Value);
            appointmentDto.Doctor = mapper.Map<DoctorDto>(doctorResult.Value);

            appointmentDtos.Add(appointmentDto);
        }

        return Result<List<AppointmentDto>>.Success(appointmentDtos);
    }
}