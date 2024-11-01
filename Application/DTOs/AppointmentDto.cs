
namespace Application.DTOs
{
    public class AppointmentDto
    {
        public Guid Id { get; set; }
        public DateTime Date { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public TimeSpan Duration { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; }
        public string? UserNotes { get; set; }
        public DateTime? CanceledAt { get; set; }
        public string? CancellationReason { get; set; }
        public string? RoomNo { get; set; }
        public string? Floor { get; set; }
    }
}
