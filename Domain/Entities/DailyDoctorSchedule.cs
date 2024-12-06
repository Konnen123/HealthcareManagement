using Domain.Entities.User;

namespace Domain.Entities;

public class DailyDoctorSchedule
{
    public Guid DailyDoctorScheduleId { get; set; }
    public DayOfWeek DayOfWeek { get; set; }
    public TimeOnly StartingTime { get; set; }
    public TimeOnly EndingTime { get; set; }
    public int SlotDurationMinutes { get; set; }
    public Guid DoctorId { get; set; }
    public Doctor? Doctor { get; set; }
    public Guid LocationId { get; set; }
    public Location? Location { get; set; }
}