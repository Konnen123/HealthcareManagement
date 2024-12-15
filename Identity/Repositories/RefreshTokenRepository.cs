using Domain.Entities.User;
using Domain.Errors;
using Domain.Repositories;
using Domain.Utils;
using Identity.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Identity.Repositories;

public class RefreshTokenRepository : IRefreshTokenRepository
{
    private readonly UsersDbContext _dbContext;

    public RefreshTokenRepository(UsersDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    public async Task<Result<RefreshToken>> AddRefreshTokenAsync(RefreshToken token)
    {
        try
        {
            await _dbContext.RefreshTokens.AddAsync(token);
            await _dbContext.SaveChangesAsync();
            return Result<RefreshToken>.Success(token);
        }
        catch (Exception e)
        {
            return Result<RefreshToken>.Failure(EntityErrors.CreateFailed(nameof(RefreshToken), e.InnerException?.Message ?? "An unexpected error occurred while creating the refresh token"));
        }
    }

    public async Task<Result<RefreshToken>> GetRefreshTokenAsync(string token)
    {
        try
        {
            var refreshToken = await _dbContext.RefreshTokens.FirstOrDefaultAsync(t => t.Token == token);
            if (refreshToken == null)
            {
                return Result<RefreshToken>.Failure(EntityErrors.GetFailed(nameof(RefreshToken), "The given refresh token does not exist"));
            }

            return Result<RefreshToken>.Success(refreshToken);
        }
        catch (Exception e)
        {
            return Result<RefreshToken>.Failure(EntityErrors.GetFailed(nameof(RefreshToken), e.InnerException?.Message ?? "An unexpected error occurred while retrieving the refresh token"));
        }
    }

    public async Task<Result<RefreshToken>> UpdateAsync(RefreshToken token)
    {
        try
        {
            _dbContext.Entry(token).State = EntityState.Modified;
            await _dbContext.SaveChangesAsync();
            return Result<RefreshToken>.Success(token);
        }
        catch (Exception e)
        {
            return Result<RefreshToken>.Failure(EntityErrors.UpdateFailed(nameof(RefreshToken), e.Message));
        }
    }

    public async Task<Result<IEnumerable<RefreshToken>>> GetByUserIdAsync(Guid userId)
    {
        try
        {
            var refreshTokens = await _dbContext.RefreshTokens.Where(t => t.UserId == userId).ToListAsync();
            return Result<IEnumerable<RefreshToken>>.Success(refreshTokens);
        }
        catch (Exception e)
        {
            return Result<IEnumerable<RefreshToken>>.Failure(EntityErrors.GetFailed(nameof(RefreshToken), e.InnerException?.Message ?? "An unexpected error occurred while retrieving the refresh tokens"));
        }
    }
}