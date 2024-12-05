using Domain.Entities;
using Domain.Utils;

namespace Domain.Repositories
{
    public interface IUsersRepository
    {
        Task<Result<Guid>> Register(User user, CancellationToken cancellationToken);
        Task<Result<string>> Login(User user, CancellationToken cancellationToken);
    }
}
