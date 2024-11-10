using Application.DTOs;
using Domain.Utils;
using MediatR;

namespace Application.Use_Cases.Queries.DailyDoctorSchedulesQueries
{
    public class GetAllSchedulesQuery : IRequest<Result<ICollection<DailyDoctorScheduleDto>>>
    {
    }
}
