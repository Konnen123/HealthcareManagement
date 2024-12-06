using Domain.Entities.User;

namespace Domain.Services;

public interface ITokenService
{
    string GenerateAccessToken(UserAuthentication user);
}