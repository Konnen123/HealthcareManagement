namespace Application.DTOs;

public class LocationDto
{
    public Guid LocationId { get; set; }
    public int RoomNo { get; set; }
    public int Floor { get; set; }
    public string? Indications { get; set; }
}