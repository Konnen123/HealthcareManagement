using Domain.Utils;
using MediatR;

namespace Application.Use_Cases.Commands.AuthCommands;

public sealed class LogoutCommand : IRequest<Result<Unit>>;