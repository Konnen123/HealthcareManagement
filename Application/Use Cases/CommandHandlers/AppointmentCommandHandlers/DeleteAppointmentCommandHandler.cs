using Application.Use_Cases.Commands;
using Application.Use_Cases.Commands.AppointmentCommands;
using AutoMapper;
using Domain.Errors;
using Domain.Repositories;
using Domain.Utils;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
                return Result<Unit>.Failure(AppointmentErrors.NotFound(request.AppointmentId));
            }

            var deleteResult = await _repository.DeleteAsync(request.AppointmentId);
            return deleteResult.IsSuccess
            ? Result<Unit>.Success(Unit.Value)
            : Result<Unit>.Failure(AppointmentErrors.DeleteFailed("Failed to delete the appointment"));
        }
    }
}
