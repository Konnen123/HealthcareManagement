using Domain.Entities.User;
using Domain.Utils;
using MediatR;

namespace Domain.Repositories
{
    public interface IUsersRepository
    {
        Task<Result<Guid>> Register(User user, CancellationToken cancellationToken);
        Task<Result<string>> Login(User user, CancellationToken cancellationToken);
        Task<Result<User>> GetByEmailAsync(string email, CancellationToken cancellationToken);
        Task<Result<User>> GetByIdAsync(Guid id, CancellationToken cancellationToken);
        Task<Result<Unit>> UpdateUserPasswordAsync(User user, CancellationToken cancellationToken);
        Task<Result<Unit>> VerifyEmailAsync(User user, CancellationToken cancellationToken);
    }
}