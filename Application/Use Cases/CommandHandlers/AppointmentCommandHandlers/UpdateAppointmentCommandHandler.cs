using Application.Use_Cases.Commands.AppointmentCommands;
using AutoMapper;
using Domain.Entities;
using Domain.Repositories;
using Domain.Utils;
using MediatR;

namespace Application.Use_Cases.CommandHandlers.AppointmentCommandHandlers
{
    public class UpdateAppointmentCommandHandler : IRequestHandler<UpdateAppointmentCommand, Result<Unit>>
    {
        private readonly IMapper _mapper;
        private readonly IAppointmentRepository _repository;

        public UpdateAppointmentCommandHandler(IMapper mapper, IAppointmentRepository repository)
        {
            _mapper = mapper;
            _repository = repository;
        }

        public async Task<Result<Unit>> Handle(UpdateAppointmentCommand request, CancellationToken cancellationToken)
        {

            var appointment = _mapper.Map<Appointment>(request);
            var resultObject = await _repository.UpdateAsync(appointment);
            return resultObject.Match<Result<Unit>>(
                onSuccess: value => Result<Unit>.Success(Unit.Value),
                onFailure: error => Result<Unit>.Failure(error));
        }
    }
}
