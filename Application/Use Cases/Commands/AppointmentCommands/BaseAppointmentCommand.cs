namespace Application.Use_Cases.Commands;

public abstract class BaseAppointmentCommand
{
    public Guid PatientId { get; set; }
    public DateOnly Date { get; set; }
    public TimeOnly StartTime { get; set; }
    public TimeOnly EndTime { get; set; }
    public string? UserNotes { get; set; }
    public Guid DoctorId { get; set; }
}