
using Domain.Utils;
using MediatR;

namespace Domain.Repositories
{
    public interface IFailedLoginAttemptsRepository
    {
        Task<Result<Unit>> AddFailedAttemptAsync(Guid userId);
        Task<Result<Boolean>> IsUserLockedOut(Guid userId);
        Task<Result<Unit>> ResetFailedAttemptsAsync(Guid userId);
    }
}
