using Domain.Entities.User;
using Domain.Utils;

namespace Domain.Repositories;

public interface IRefreshTokenRepository
{
    Task<Result<RefreshToken>> AddRefreshTokenAsync(RefreshToken token);
    Task<Result<RefreshToken>> GetRefreshTokenAsync(string token);
    Task<Result<RefreshToken>> UpdateAsync(RefreshToken token);
    Task<Result<IEnumerable<RefreshToken>>> GetByUserIdAsync(Guid userId);
}