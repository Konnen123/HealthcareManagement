using Domain.Entities.Tokens;
using Domain.Errors;
using Domain.Repositories;
using Domain.Utils;
using Identity.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Identity.Repositories;

public class VerifyEmailTokenRepository : IVerifyEmailTokenRepository
{
    private readonly UsersDbContext _context;

    public VerifyEmailTokenRepository(UsersDbContext context)
    {
        _context = context;
    }

    public async Task<Result<Unit>> AddVerifyEmailTokenAsync(VerifyEmailToken token)
    {
        try
        {
            await _context.VerifyEmailTokens.AddAsync(token);
            await _context.SaveChangesAsync();
            return Result<Unit>.Success(Unit.Value);
        }
        catch (Exception e)
        {
            return Result<Unit>.Failure(EntityErrors.CreateFailed(nameof(VerifyEmailToken), e.InnerException?.Message ?? "An error occurred while adding the verify email token"));
        }
    }

    public async Task<Result<VerifyEmailToken>> GetVerifyEmailTokenAsync(string token)
    {
        try
        {
            var verifyEmailToken = await _context.VerifyEmailTokens.FirstOrDefaultAsync(t => t.Token == token);
            if (verifyEmailToken == null)
            {
                return Result<VerifyEmailToken>.Failure(EntityErrors.GetFailed(nameof(VerifyEmailToken), "Verify email token does not exist"));
            }

            return Result<VerifyEmailToken>.Success(verifyEmailToken);
        }
        catch (Exception e)
        {
            return Result<VerifyEmailToken>.Failure(EntityErrors.GetFailed(nameof(VerifyEmailToken), e.InnerException?.Message ?? "An unexpected error occurred while retrieving the verify email token"));                                                           
        }
    }

    public async Task<Result<Unit>> DeleteByUserIdAsync(Guid id)
    {
        try
        {
            var verifyEmailToken = await _context.VerifyEmailTokens.FirstOrDefaultAsync(t => t.UserId == id);
            if (verifyEmailToken == null)
            {
                return Result<Unit>.Failure(EntityErrors.DeleteFailed(nameof(VerifyEmailToken), "Verify email token does not exist"));
            }

            _context.VerifyEmailTokens.Remove(verifyEmailToken);
            await _context.SaveChangesAsync();
            return Result<Unit>.Success(Unit.Value);
        }
        catch (Exception e)
        {
            return Result<Unit>.Failure(EntityErrors.DeleteFailed(nameof(VerifyEmailToken), e.InnerException?.Message ?? "An unexpected error occurred while deleting the verify email token"));
        }
    }
}