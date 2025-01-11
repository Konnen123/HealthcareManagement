
using Domain.Utils;

namespace Domain.Repositories
{
    public interface IFailedLoginAttemptsRepository
    {
        Task AddFailedAttemptAsync(Guid userId);
        Task<Result<Boolean>> IsUserLockedOut(Guid userId);
        Task ResetFailedAttemptsAsync(Guid userId);
    }
}
