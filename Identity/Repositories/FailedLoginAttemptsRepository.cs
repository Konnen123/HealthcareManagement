using Domain.Entities.User;
using Domain.Errors;
using Domain.Repositories;
using Domain.Utils;
using Identity.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Identity.Repositories
{
    public class FailedLoginAttemptsRepository : IFailedLoginAttemptsRepository
    {
        private readonly UsersDbContext _context;
        private readonly ILogger<FailedLoginAttemptsRepository> _logger;

        public FailedLoginAttemptsRepository(UsersDbContext context, ILogger<FailedLoginAttemptsRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task AddFailedAttemptAsync(Guid userId)
        {
            try
            {
                var failedAttempt = await _context.FailedLoginAttempts
                    .SingleOrDefaultAsync(f => f.UserId == userId);

                if (failedAttempt == null)
                {
                    failedAttempt = new FailedLoginAttempt
                    {
                        UserId = userId,
                        FailedAttempts = 1,
                        LastFailedAttemptTime = DateTime.UtcNow,
                        LockoutEndTime = null
                    };
                    await _context.FailedLoginAttempts.AddAsync(failedAttempt);
                }
                else
                {
                    failedAttempt.FailedAttempts++;
                    failedAttempt.LastFailedAttemptTime = DateTime.UtcNow;

                    if (failedAttempt.FailedAttempts >= failedAttempt.MaxFailedLoginAttempts)
                    {
                        failedAttempt.LockoutEndTime = DateTime.UtcNow.AddSeconds(40);
                    }

                    _context.FailedLoginAttempts.Update(failedAttempt);
                }

                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while adding a failed login attempt for user {UserId}", userId);
                throw; // Re-throw the exception to notify the caller
            }
        }

        public async Task<Result<bool>> IsUserLockedOut(Guid userId)
        {
            try
            {
                var failedAttempt = await _context.FailedLoginAttempts
                    .SingleOrDefaultAsync(f => f.UserId == userId);

                if (failedAttempt == null || failedAttempt.LockoutEndTime == null)
                {
                    return Result<bool>.Success(false); // User is not locked out
                }

                if (failedAttempt.LockoutEndTime > DateTime.UtcNow)
                {
                    return Result<bool>.Success(true); // User is locked out
                }

                // Lockout period has expired, reset lockout data
                failedAttempt.LockoutEndTime = null;
                failedAttempt.FailedAttempts = 0;
                _context.FailedLoginAttempts.Update(failedAttempt);
                await _context.SaveChangesAsync();

                return Result<bool>.Success(false); // Lockout expired, user is not locked out
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while checking lockout status for user {UserId}", userId);
                return Result<bool>.Failure(AuthErrors.LoginFailed(nameof(FailedLoginAttempt),"An error occurred while processing the request.")); // Provide error message
            }
        }

        public async Task ResetFailedAttemptsAsync(Guid userId)
        {
            try
            {
                var failedAttempt = await _context.FailedLoginAttempts
                    .SingleOrDefaultAsync(f => f.UserId == userId);

                if (failedAttempt != null)
                {
                    _context.FailedLoginAttempts.Remove(failedAttempt);
                    await _context.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while resetting failed login attempts for user {UserId}", userId);
                throw;
            }
        }
    }
}
