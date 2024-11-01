using Domain.Utils;
using MediatR;

namespace Application.Use_Cases.Commands;

public class CreateAppointmentCommand : BaseAppointmentCommand, IRequest<Result<Guid>>;