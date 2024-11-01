using Application.DTOs;
using Application.Use_Cases.Commands;
using AutoMapper;
using Domain.Entities;

namespace Application.Utils;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Appointment, AppointmentDto>().ReverseMap();
        CreateMap<CreateAppointmentCommand, Appointment>().ReverseMap();
        // CreateMap<UpdateAppointmentCommand, AppointmentDto>().ReverseMap();
    }
}