namespace Domain.Services;

public interface IPasswordHashingService
{
    public string HashPassword(string password);
    public bool VerifyPassword(string password, string passwordHash);
}