

namespace Application.Use_Cases.Commands.AppointmentCommands
{
    public abstract class CancelIdCommand
    {
        public Guid PatientId { get; set; }
        public Guid AppointmentId { get; set; } 
       
    }
}
