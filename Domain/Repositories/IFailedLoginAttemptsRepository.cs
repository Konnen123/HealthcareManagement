
namespace Domain.Repositories
{
    public interface IFailedLoginAttemptsRepository
    {
        void AddFailedAttempt(Guid userId);
        bool IsUserLockedOut(Guid userId);
        void ResetFailedAttempts(Guid userId);
    }
}
