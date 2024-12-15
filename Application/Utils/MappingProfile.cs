using Application.DTOs;
using Application.DTOs.UserDto;
using Application.Use_Cases.Commands;
using Application.Use_Cases.Commands.AppointmentCommands;
using Application.Use_Cases.Commands.AuthCommands;
using Application.Use_Cases.Commands.LocationCommands;
using Application.Use_Cases.Commands.SchedulesCommands;
using AutoMapper;
using Domain.Entities;
using Domain.Entities.User;

namespace Application.Utils;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Appointment, AppointmentDto>().ReverseMap();
        CreateMap<CreateAppointmentCommand, Appointment>().ReverseMap();
        CreateMap<UpdateAppointmentCommand, Appointment>().ReverseMap();
        CreateMap<RescheduleAppointmentCommand, AppointmentUpdateRequest>().ReverseMap();
        CreateMap<CreateLocationCommand, Location>().ReverseMap();
        CreateMap<Location, LocationDto>().ReverseMap();
        CreateMap<DailyDoctorSchedule, DailyDoctorScheduleDto>().ReverseMap();
        CreateMap<CreateDailyDoctorScheduleCommand, DailyDoctorSchedule>().ReverseMap();
        CreateMap<UpdateDailyDoctorScheduleCommand, DailyDoctorSchedule>().ReverseMap();
        CreateMap<RegisterUserCommand, Patient>().ReverseMap();
        CreateMap<RegisterUserCommand, Doctor>().ReverseMap();
        CreateMap<Doctor, DoctorDto>().ReverseMap();
        CreateMap<Patient, PatientDto>().ReverseMap();
    }
}