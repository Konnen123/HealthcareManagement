namespace Domain.Entities
{
    public class Appointment
    {
        public Guid Id { get; set; }
        
        public DateTime Date { get; set; }

        public DateTime StartTime { get; set; }

        public DateTime EndTime { get; set; }

        public TimeSpan Duration => EndTime - StartTime;

        
        public string? UserNotes { get; set; }

        public Patient? Patient { get; set; }
        public Guid PatientId { get; set; }

        public Doctor? Doctor { get; set; }
        public Guid DoctorId { get; set; }

        public string? RoomNo { get; set; }
        public string? Floor { get; set; }
        
        public DateTime? CanceledAt { get; set; }
        public string? CancellationReason { get; set; }
        
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    }
}
