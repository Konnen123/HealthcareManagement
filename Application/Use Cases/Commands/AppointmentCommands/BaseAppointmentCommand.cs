namespace Application.Use_Cases.Commands;

public abstract class BaseAppointmentCommand
{
    public Guid PatientId { get; set; }
    public DateTime Date { get; set; }
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
    public string? UserNotes { get; set; }
    public Guid DoctorId { get; set; }
}