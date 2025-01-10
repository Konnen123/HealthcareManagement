namespace Domain.Entities.User
{
    public class UserAuthentication : User
    {
        public ICollection<RefreshToken> RefreshTokens { get; set; } = new List<RefreshToken>();
        public ResetPasswordToken ResetPasswordToken { get; set; } = null!;
    }
}
