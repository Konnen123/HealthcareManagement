using Domain.Entities.User;
using Domain.Errors;
using Domain.Repositories;
using Domain.Utils;
using Identity.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Identity.Repositories
{
    public class UsersRepository : IUsersRepository
    {
        private readonly UsersDbContext _context;

        public UsersRepository(UsersDbContext context)
        {
            _context = context;
        }

        public async Task<Result<string>> Login(User user, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public async Task<Result<User>> GetByEmailAsync(string email, CancellationToken cancellationToken)
        {
            try
            {
                var account = await _context.Users.FirstOrDefaultAsync(x => x.Email == email, cancellationToken);
                return account == null ? Result<User>.Failure(AuthErrors.EmailNotFound(nameof(User), email)) : Result<User>.Success(account);
            }
            catch (Exception e)
            {
                return Result<User>.Failure(EntityErrors.GetFailed(nameof(User), e.InnerException?.Message ?? $"An unexpected error occurred while retrieving the account with email {email}"));
            }
        }

        public async Task<Result<User>> GetByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            try
            {
                var account = await _context.Users.FirstOrDefaultAsync(x => x.UserId == id, cancellationToken);
                return account == null ? Result<User>.Failure(EntityErrors.NotFound(nameof(User), id)) : Result<User>.Success(account);
            }
            catch (Exception e)
            {
                return Result<User>.Failure(EntityErrors.GetFailed(nameof(User), e.InnerException?.Message ?? $"An unexpected error occurred while retrieving the account with id {id}"));
            }
        }

        public async Task<Result<Guid>> Register(User user, CancellationToken cancellationToken)
        {
            try
            {
                await _context.Users.AddAsync(user);
                await _context.SaveChangesAsync(cancellationToken);
                return Result<Guid>.Success(user.UserId);
            }
            catch (Exception e)
            {
                return Result<Guid>.Failure(EntityErrors.CreateFailed(nameof(User),e.InnerException?.Message ?? "An unexpected error occurred while creating the user account"));
            }
        }
    }
}

