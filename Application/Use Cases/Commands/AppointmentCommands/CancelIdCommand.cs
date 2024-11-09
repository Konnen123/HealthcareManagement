using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Use_Cases.Commands.AppointmentCommands
{
    public abstract class CancelIdCommand
    {
        public Guid PatientId { get; set; }
        public Guid AppointmentId { get; set; } 
       
    }
}
