namespace Application.Use_Cases.Commands.SchedulesCommands
{
    public abstract class BaseDailyDoctorScheduleCommand
    {
        public DayOfWeek DayOfWeek { get; set; }
        public TimeOnly StartingTime { get; set; }
        public TimeOnly EndingTime { get; set; }
        public int SlotDurationMinutes { get; set; }
        public Guid DoctorId { get; set; }
        public Guid LocationId { get; set; }
    }
}
