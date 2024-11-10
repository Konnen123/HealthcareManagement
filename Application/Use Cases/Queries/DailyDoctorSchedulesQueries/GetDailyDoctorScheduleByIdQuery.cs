using Application.DTOs;
using Application.Use_Cases.Commands;
using Domain.Utils;
using MediatR;


namespace Application.Use_Cases.Queries.DailyDoctorSchedulesQueries
{
    public class GetDailyDoctorScheduleByIdQuery : IdCommand, IRequest<Result<DailyDoctorScheduleDto>>
    {
    }
}
