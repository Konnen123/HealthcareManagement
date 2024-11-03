using Application.DTOs;
using Domain.Utils;
using MediatR;

namespace Application.Use_Cases.Queries.AppointmentQueries
{
    public class GetAllAppointmentsQuery : IRequest<Result<List<AppointmentDto>>>
    {
    }
} 