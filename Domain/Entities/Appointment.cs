namespace Domain.Entities
{
    public class Appointment
    {
        public Guid Id { get; set; }
        
        public DateOnly Date { get; set; }

        public TimeOnly StartTime { get; set; }

        public TimeOnly EndTime { get; set; }

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

        public ICollection<AppointmentUpdateRequest> UpdateRequests { get; set; } =
            new List<AppointmentUpdateRequest>();
    }
}
