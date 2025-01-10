namespace Domain.Entities.User;

public class ResetPasswordToken
{
    public Guid ResetPasswordTokenId { get; set; }
    
    public string Token { get; set; } = null!;
    
    public Guid UserId { get; set; }
    
    public User UserAuthentication { get; set; } = null!;
    
    public DateTime CreatedAt { get; set; }
    
    public DateTime ExpiresAt { get; set; }
    
}