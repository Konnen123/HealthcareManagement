namespace Domain.Entities.User
{
    public class Doctor : Staff
    {
        public ICollection<DailyDoctorSchedule>? DailySchedules { get; set; }
        public ICollection<ScheduleIrregularity>? ScheduleIrregularities { get; set; }
    }
}
