using Domain.Entities.User;
using Domain.Errors;
using Domain.Repositories;
using Domain.Utils;
using Identity.Persistence;
using MediatR;
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

        public async Task<Result<Unit>> AddFailedAttemptAsync(Guid userId)
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
                return Result<Unit>.Success(Unit.Value);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while adding a failed login attempt for user {UserId}", userId);
                return Result<Unit>.Failure(AuthErrors.LoginFailed(nameof(FailedLoginAttempt),
                    "An error occurred while adding a failed login attempt"));
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
                    return Result<bool>.Success(false);
                }

                if (failedAttempt.LockoutEndTime > DateTime.UtcNow)
                {
                    return Result<bool>.Success(true);
                }
                
                failedAttempt.LockoutEndTime = null;
                failedAttempt.FailedAttempts = 0;
                _context.FailedLoginAttempts.Update(failedAttempt);
                await _context.SaveChangesAsync();

                return Result<bool>.Success(false);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while checking lockout status for user {UserId}", userId);
                return Result<bool>.Failure(AuthErrors.LoginFailed(nameof(FailedLoginAttempt),"An error occurred while processing the request.")); // Provide error message
            }
        }

        public async Task<Result<Unit>> ResetFailedAttemptsAsync(Guid userId)
        {
            try
            {
                var failedAttempt = await _context.FailedLoginAttempts
                    .SingleOrDefaultAsync(f => f.UserId == userId);

                if (failedAttempt != null)
                {
                    _context.FailedLoginAttempts.Remove(failedAttempt);
                    await _context.SaveChangesAsync();
                    return Result<Unit>.Success(Unit.Value);
                }
                return Result<Unit>.Failure(AuthErrors.LoginFailed(nameof(FailedLoginAttempt),"Failed login attempt not found"));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while resetting failed login attempts for user {UserId}", userId);
                return Result<Unit>.Failure(AuthErrors.LoginFailed(nameof(FailedLoginAttempt),
                    "An error occurred while resetting a failed login attempt"));
            }
        }
    }
}
