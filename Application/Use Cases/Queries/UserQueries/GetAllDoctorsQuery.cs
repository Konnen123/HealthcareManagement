using Application.DTOs.UserDto;
using Domain.Utils;
using MediatR;

namespace Application.Use_Cases.Queries.UserQueries
{
    public class GetAllDoctorsQuery: IRequest<Result<List<DoctorDto>>>
    {

    }
}
