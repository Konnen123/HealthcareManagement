using Domain.Entities.User;
using Domain.Errors;
using Domain.Repositories;
using Domain.Utils;
using Identity.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Identity.Repositories
{
    public class PatientsRepository : IPatientsRepository
    {
        private readonly UsersDbContext context;

        public PatientsRepository(UsersDbContext context)
        {
            this.context = context;
        }

        public async Task<Result<Patient>> GetByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            try
            {
                var account = await context.Patients.FirstOrDefaultAsync(x => x.UserId == id, cancellationToken);
                return account == null ? 
                    Result<Patient>.Failure(EntityErrors.NotFound(nameof(Patient), id)) : 
                    Result<Patient>.Success(account);
            }
            catch (Exception e)
            {
                return Result<Patient>.Failure(EntityErrors.GetFailed(nameof(Patient), e.InnerException?.Message ?? $"An unexpected error occurred while retrieving the account with id {id}"));
            }
        }
    }
}
