namespace Domain.Entities.User;
public class RefreshToken
{
    public Guid RefreshTokenId { get; set; }
    public string Token { get; set; }
    public User User { get; set; }
    public Guid UserId { get; set; }
    public DateTime IssuedAt { get; set; }
    public DateTime ExpiresAt { get; set; }
    public string? DeviceInfo { get; set; }
    public string? IpAddress { get; set; }
    public bool IsRevoked { get; set; }
    public DateTime? RevokedAt { get; set; }
}
