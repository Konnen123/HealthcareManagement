namespace Domain.Entities;

public class AppointmentUpdateRequest
{
    public Guid Id { get; set; }
    public Guid AppointmentId { get; set; }
    public DateTime? NewDate { get; set; }
    public DateTime? NewStartTime { get; set; }
    public bool IsProcessed { get; set; } = false;
    public bool IsAccepted { get; set; } = false;
    public Appointment Appointment { get; set; }
}