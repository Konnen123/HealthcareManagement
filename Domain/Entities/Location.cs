namespace Domain.Entities;

public class Location
{
    public Guid LocationId { get; set; }
    public int RoomNo { get; set; }
    public int Floor { get; set; }
    public string? Indications { get; set; }
    public ICollection<DailyDoctorSchedule>? DoctorSchedules { get; set; }
}