using Domain.Entities;
using Domain.Utils;
using MediatR;

namespace Application.Use_Cases.Commands.LocationCommands;

public class BulkInsertLocationCommand : IRequest<Result<Unit>>
{
    public int MaxFloorNo { get; set; }
    public int RoomsPerFloor { get; set; }
}