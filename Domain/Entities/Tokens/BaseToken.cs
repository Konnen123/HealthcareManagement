namespace Domain.Entities.Tokens;

public abstract class BaseToken
{
    public string Token { get; set; } = null!;
    
    public Guid UserId { get; set; }
    
    public User.User UserAuthentication { get; set; } = null!;
    
    public DateTime CreatedAt { get; set; }
    
    public DateTime ExpiresAt { get; set; }
}