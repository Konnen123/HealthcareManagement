using Domain.Entities.User;

namespace Domain.Entities.Tokens;

public class ResetPasswordToken : BaseToken
{
    public Guid ResetPasswordTokenId { get; set; }
}