using Application.Use_Cases.Commands;
using Application.Use_Cases.Commands.AppointmentCommands;
using AutoMapper;
using Domain.Entities;
using Domain.Errors;
using Domain.Repositories;
using Domain.Utils;
using MediatR;

namespace Application.Use_Cases.CommandHandlers.AppointmentCommandHandlers
{
    public class DeleteAppointmentCommandHandler : IRequestHandler<DeleteAppointmentCommand, Result<Unit>>
    {
        private readonly IMapper _mapper;
        private readonly IAppointmentRepository _repository;

        public DeleteAppointmentCommandHandler(IMapper mapper, IAppointmentRepository repository)
        {
            _mapper = mapper;
            _repository = repository;
        }

        public async Task<Result<Unit>> Handle(DeleteAppointmentCommand request, CancellationToken cancellationToken)
        {
            var existingAppointmentResult = await _repository.GetAsync(request.AppointmentId);
            if (!existingAppointmentResult.IsSuccess)
            {
                return Result<Unit>.Failure(EntityErrors.NotFound(nameof(Appointment), request.AppointmentId));
            }

            var deleteResult = await _repository.DeleteAsync(request.AppointmentId);
            return deleteResult.IsSuccess
            ? Result<Unit>.Success(Unit.Value)
            : Result<Unit>.Failure(EntityErrors.DeleteFailed(nameof(Appointment), "Failed to delete the appointment"));
        }
    }
}
