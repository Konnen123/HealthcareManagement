namespace Application.DTOs
{
    public class DailyDoctorScheduleDto
    {
        public Guid DailyDoctorScheduleId{ get; set; }
        public DayOfWeek DayOfWeek { get; set; }
        public TimeOnly StartingTime { get; set; }
        public TimeOnly EndingTime { get; set; }
        public int SlotDurationMinutes { get; set; }
        public Guid DoctorId { get; set; }
        public Guid LocationId { get; set; }
    }
}
