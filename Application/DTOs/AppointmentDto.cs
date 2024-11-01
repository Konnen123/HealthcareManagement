
namespace Application.DTOs
{
    public class AppointmentDto
    {
        public Guid Id { get; set; }

        public DateOnly Date { get; set; }

        public TimeOnly StartTime { get; set; }

        public TimeOnly EndTime { get; set; }
    }
}
