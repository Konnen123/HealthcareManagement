using Domain.Entities.User;

namespace Domain.Entities.Tokens;

public class VerifyEmailToken : BaseToken
{
    public Guid VerifyEmailTokenId { get; set; }
}