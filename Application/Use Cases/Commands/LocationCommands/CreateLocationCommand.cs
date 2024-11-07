using Domain.Utils;
using MediatR;

namespace Application.Use_Cases.Commands.LocationCommands;

public class CreateLocationCommand : IRequest<Result<Guid>>
{
    public int RoomNo { get; set; }
    public int Floor { get; set; }
    public string? Indications { get; set; }
}