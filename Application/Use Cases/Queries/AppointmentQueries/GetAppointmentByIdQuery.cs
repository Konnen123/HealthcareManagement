using Application.DTOs;
using Application.Use_Cases.Commands;
using Domain.Utils;
using MediatR;

namespace Application.Use_Cases.Queries.AppointmentQueries;

public class GetAppointmentByIdQuery : IdCommand, IRequest<Result<AppointmentDto>>
{
}