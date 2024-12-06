using Domain.Entities.User;
using Domain.Utils;

namespace Domain.Repositories
{
    public interface IUsersRepository
    {
        Task<Result<Guid>> Register(UserAuthentication user, CancellationToken cancellationToken);
        Task<Result<string>> Login(UserAuthentication user, CancellationToken cancellationToken);
        Task<Result<UserAuthentication>> GetByEmailAsync(string email, CancellationToken cancellationToken);
    }
}
