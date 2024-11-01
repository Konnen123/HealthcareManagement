using Application.DTOs;
using Application.Use_Cases.Queries.AppointmentQueries;
using AutoMapper;
using Domain.Repositories;
using Domain.Utils;
using MediatR;

namespace Application.Use_Cases.QueryHandlers.AppointmentQueryHandlers;

public class GetAppointmentByIdQueryHandler : IRequestHandler<GetAppointmentByIdQuery, Result<AppointmentDto>>
{
    private readonly IMapper _mapper;
    private readonly IAppointmentRepository _repository;

    public GetAppointmentByIdQueryHandler(IMapper mapper, IAppointmentRepository repository)
    {
        _mapper = mapper;
        _repository = repository;
    }
    
    public async Task<Result<AppointmentDto>> Handle(GetAppointmentByIdQuery request, CancellationToken cancellationToken)
    {
        var resultObject = await _repository.GetAsync(request.Id);
        return resultObject.Match(
            onSuccess: value => Result<AppointmentDto>.Success(_mapper.Map<AppointmentDto>(value)),
            onFailure: error => Result<AppointmentDto>.Failure(error)
        );
    }
}