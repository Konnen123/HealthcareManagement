using Application.DTOs;
using Application.Use_Cases.Commands;
using Application.Use_Cases.Commands.AppointmentCommands;
using Application.Use_Cases.Commands.LocationCommands;
using AutoMapper;
using Domain.Entities;

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
    }
}