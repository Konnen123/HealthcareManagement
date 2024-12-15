using Domain.Entities.User;
using Domain.Utils;

namespace Domain.Repositories
{
    public interface IDoctorsRepository
    {
        Task<Result<Doctor>> GetByIdAsync(Guid id, CancellationToken cancellationToken);
    }
}
