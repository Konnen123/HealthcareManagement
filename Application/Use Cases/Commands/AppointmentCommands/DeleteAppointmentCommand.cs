using Domain.Utils;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Use_Cases.Commands.AppointmentCommands
{
    public class DeleteAppointmentCommand : IRequest<Result<Unit>>
    {
        public Guid AppointmentId { get; set; }
    }
}
