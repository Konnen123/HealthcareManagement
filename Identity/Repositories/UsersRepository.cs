using Domain.Entities.User;
using Domain.Errors;
using Domain.Repositories;
using Domain.Utils;
using Identity.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Identity.Repositories
{
    public class UsersRepository : IUsersRepository
    {
        private readonly IConfiguration configuration;
        private readonly UsersDbContext _context;

        public UsersRepository(IConfiguration configuration, UsersDbContext context)
        {
            this.configuration = configuration;
            _context = context;
        }

        public async Task<Result<string>> Login(UserAuthentication user, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public async Task<Result<UserAuthentication>> GetByEmailAsync(string email, CancellationToken cancellationToken)
        {
            try
            {
                var account = await _context.Users.FirstOrDefaultAsync(x => x.Email == email, cancellationToken);
                return account == null ? Result<UserAuthentication>.Failure(AuthErrors.EmailNotFound(nameof(UserAuthentication), email)) : Result<UserAuthentication>.Success(account);
            }
            catch (Exception e)
            {
                return Result<UserAuthentication>.Failure(EntityErrors.GetFailed(nameof(UserAuthentication), e.InnerException?.Message ?? $"An unexpected error occurred while retrieving the account with email {email}"));
            }
        }

        public async Task<Result<Guid>> Register(UserAuthentication user, CancellationToken cancellationToken)
        {
            Console.WriteLine(user.Password.Length);
            try
            {
                await _context.Users.AddAsync(user);
                await _context.SaveChangesAsync(cancellationToken);
                return Result<Guid>.Success(user.Id);
            }
            catch (Exception e)
            {
                return Result<Guid>.Failure(EntityErrors.CreateFailed(nameof(UserAuthentication),e.InnerException?.Message ?? "An unexpected error occurred while creating the user account"));
            }
        }
    }
}

