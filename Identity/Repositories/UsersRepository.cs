using Domain.Entities.User;
using Domain.Errors;
using Domain.Repositories;
using Domain.Utils;
using Identity.Persistence;
using MediatR;
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
                return account == null
                    ? Result<User>.Failure(AuthErrors.EmailNotFound(nameof(User), email))
                    : Result<User>.Success(account);
            }
            catch (Exception e)
            {
                return Result<User>.Failure(EntityErrors.GetFailed(nameof(User),
                    e.InnerException?.Message ??
                    $"An unexpected error occurred while retrieving the account with email {email}"));
            }
        }

        public async Task<Result<User>> GetByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            try
            {
                var account = await _context.Users.FirstOrDefaultAsync(x => x.UserId == id, cancellationToken);
                return account == null
                    ? Result<User>.Failure(EntityErrors.NotFound(nameof(User), id))
                    : Result<User>.Success(account);
            }
            catch (Exception e)
            {
                return Result<User>.Failure(EntityErrors.GetFailed(nameof(User),
                    e.InnerException?.Message ??
                    $"An unexpected error occurred while retrieving the account with id {id}"));
            }
        }

        public async Task<Result<Unit>> UpdateUserPasswordAsync(User user,
            CancellationToken cancellationToken)
        {
            try
            {
                _context.Users.Update(user);
                await _context.SaveChangesAsync(cancellationToken);

                return Result<Unit>.Success(Unit.Value);
            }
            catch (Exception ex)
            {
                return Result<Unit>.Failure(
                    EntityErrors.UpdateFailed(nameof(User),
                        ex.InnerException?.Message ?? "An unexpected error occurred while updating the user password")
                );
            }
        }

        public async Task<Result<Guid>> Register(User user, CancellationToken cancellationToken)
        {
            try
            {
                await _context.Users.AddAsync(user, cancellationToken);
                await _context.SaveChangesAsync(cancellationToken);
                return Result<Guid>.Success(user.UserId);
            }
            catch (DbUpdateException dbEx)
            {
                if (dbEx.InnerException?.Message.Contains("IX_users_Email", StringComparison.OrdinalIgnoreCase) == true)
                {
                    return Result<Guid>.Failure(AuthErrors.EmailAlreadyExists(nameof(User), $"{user.Email}"));
                }

                return Result<Guid>.Failure(EntityErrors.CreateFailed(nameof(User),
                    dbEx.InnerException?.Message ?? "An unexpected error occurred while creating the user account"));
            }
            catch (Exception e)
            {
                return Result<Guid>.Failure(EntityErrors.CreateFailed(nameof(User), e.Message));
            }
        }
    }
}