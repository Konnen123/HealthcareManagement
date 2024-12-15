using Domain.Entities.User;

namespace Domain.Entities;

//am numit irregularity pentru ca ScheduleException suna ca o exceptie
public class ScheduleIrregularity
{
    public Guid ScheduleIrregularityId { get; set; }
    public DateOnly Date { get; set; }
    public TimeOnly StartTime { get; set; }
    public TimeOnly EndTime { get; set; }
    public string? Reason { get; set; }
    public Guid DoctorId { get; set; }
}