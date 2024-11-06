using Domain.Utils;
using MediatR;

namespace Application.Use_Cases.Commands.LocationCommands;

public class DeleteLocationCommand : IdCommand, IRequest<Result<Unit>>
{
    
}