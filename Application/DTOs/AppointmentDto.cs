using Application.DTOs.UserDto;

namespace Application.DTOs
{
    public class AppointmentDto
    {
        public Guid Id { get; set; }
        public DateOnly Date { get; set; }
        public TimeOnly StartTime { get; set; }
        public TimeOnly EndTime { get; set; }
        public TimeSpan Duration { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; }
        public string? UserNotes { get; set; }
        public required PatientDto Patient { get; set; }
        public required DoctorDto Doctor { get; set; }
        public DateTime? CanceledAt { get; set; }
        public string? CancellationReason { get; set; }
        public string? RoomNo { get; set; }
        public string? Floor { get; set; }
    }
}
