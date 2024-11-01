namespace Domain.Entities
{
    public class Appointment
    {
        public Guid Id { get; set; }

        public DateOnly Date { get; set; }

        public TimeOnly StartTime { get; set; }

        public TimeOnly EndTime { get; set; }

    }
}
