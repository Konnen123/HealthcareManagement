using Application.DTOs;
using Application.Use_Cases.Queries.AppointmentQueries;
using AutoMapper;
using Domain.Repositories;
using Domain.Utils;
using MediatR;

namespace Application.Use_Cases.QueryHandlers.AppointmentQueryHandlers;

public class GetAllAppointmentsQueryHandler : IRequestHandler<GetAllAppointmentsQuery, Result<List<AppointmentDto>>>
{
    private readonly IMapper _mapper;
    private readonly IAppointmentRepository _repository;

    public GetAllAppointmentsQueryHandler(IMapper mapper, IAppointmentRepository repository)
    {
        _mapper = mapper;
        _repository = repository;
    }

    public async Task<Result<List<AppointmentDto>>> Handle(GetAllAppointmentsQuery request, CancellationToken cancellationToken)
    {
        var resultObject = await _repository.GetAllAsync();
        return resultObject.Match(
            onSuccess: value => Result<List<AppointmentDto>>.Success(_mapper.Map<List<AppointmentDto>>(value)),
            onFailure: error => Result<List<AppointmentDto>>.Failure(error)
        );
    }
}
