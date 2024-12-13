using Domain.Entities.User;
using Domain.Utils;

namespace Domain.Repositories
{
    public interface IPatientsRepository
    {
        Task<Result<Patient>> GetByIdAsync(Guid id, CancellationToken cancellationToken);
    }
}
