using Domain.Entities.User;
using Domain.Repositories;
using Identity.Persistence;

namespace Identity.Repositories
{
    public class FailedLoginAttemptsRepository : IFailedLoginAttemptsRepository
    {
        private readonly UsersDbContext _context;

        public FailedLoginAttemptsRepository(UsersDbContext context)
        {
            _context = context;
        }

        public void AddFailedAttempt(Guid userId)
        {
            var failedAttempt = _context.FailedLoginAttempts
                .SingleOrDefault(f => f.UserId == userId);

            if (failedAttempt == null)
            {
                failedAttempt = new FailedLoginAttempt
                {
                    UserId = userId,
                    FailedAttempts = 1,
                    LastFailedAttemptTime = DateTime.UtcNow,
                    LockoutEndTime = null
                };
                _context.FailedLoginAttempts.AddAsync(failedAttempt);
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

            _context.SaveChangesAsync();
        }

        public bool IsUserLockedOut(Guid userId)
        {
            var failedAttempt = _context.FailedLoginAttempts
                .SingleOrDefault(f => f.UserId == userId);

            if (failedAttempt == null || failedAttempt.LockoutEndTime == null)
            {
                return false; 
            }

            if (failedAttempt.LockoutEndTime > DateTime.UtcNow)
            {
                return true;
            }

            failedAttempt.LockoutEndTime = null;
            failedAttempt.FailedAttempts = 0;
            _context.FailedLoginAttempts.Update(failedAttempt);
            _context.SaveChangesAsync();

            return false;
        }

        public void ResetFailedAttempts(Guid userId)
        {
            var failedAttempt = _context.FailedLoginAttempts
                .SingleOrDefault(f => f.UserId == userId);

            if (failedAttempt != null)
            {
                _context.FailedLoginAttempts.Remove(failedAttempt);
                _context.SaveChangesAsync();
            }
        }
    }
}
