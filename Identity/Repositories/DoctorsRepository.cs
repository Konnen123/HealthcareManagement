using Domain.Entities.User;
using Domain.Errors;
using Domain.Repositories;
using Domain.Utils;
using Identity.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Identity.Repositories
{
    public class DoctorsRepository : IDoctorsRepository
    {
        private readonly UsersDbContext context;

        public DoctorsRepository(UsersDbContext context)
        {
            this.context = context;
        }

        public async Task<Result<Doctor>> GetByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            try
            {
                var account = await context.Doctors.FirstOrDefaultAsync(x => x.UserId == id, cancellationToken);
                return account == null ?
                    Result<Doctor>.Failure(EntityErrors.NotFound(nameof(Doctor), id)) :
                    Result<Doctor>.Success(account);
            }
            catch (Exception e)
            {
                return Result<Doctor>.Failure(EntityErrors.GetFailed(nameof(Doctor), e.InnerException?.Message ?? $"An unexpected error occurred while retrieving the account with id {id}"));
            }
        }
    }
}
