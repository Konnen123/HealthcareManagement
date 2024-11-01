using Application.DTOs;
using Domain.Entities;
using Domain.Utils;
using MediatR;

namespace Application.Use_Cases.Queries.AppointmentQueries;

public class GetAppointmentByIdQuery : IRequest<Result<AppointmentDto>>
{
    public Guid Id { get; set; }
}