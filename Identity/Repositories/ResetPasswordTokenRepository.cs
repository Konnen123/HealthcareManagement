using Domain.Entities.Tokens;
using Domain.Entities.User;
using Domain.Errors;
using Domain.Repositories;
using Domain.Utils;
using Identity.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Identity.Repositories;

public class ResetPasswordTokenRepository : IResetPasswordTokenRepository
{
    private readonly UsersDbContext _context;

    public ResetPasswordTokenRepository(UsersDbContext context)
    {
        _context = context;
    }

    public async Task<Result<Unit>> AddResetPasswordToken(ResetPasswordToken token)
    {
        var existingToken = await _context.ResetPasswordTokens
            .FirstOrDefaultAsync(t => t.UserId == token.UserId);

        if (existingToken != null)
        {
            existingToken.Token = token.Token;
            existingToken.CreatedAt = token.CreatedAt;
            existingToken.ExpiresAt = token.ExpiresAt;
        }
        else
        {
            _context.ResetPasswordTokens.Add(token);
        }

        await _context.SaveChangesAsync();
        return Result<Unit>.Success(Unit.Value);
    }

    public async Task<Result<ResetPasswordToken>> GetByUserId(Guid userId)
    {
        try
        {
            var resetPasswordToken = await _context.ResetPasswordTokens.FirstOrDefaultAsync(t => t.UserId == userId);
            if (resetPasswordToken == null)
            {
                return Result<ResetPasswordToken>.Failure(EntityErrors.GetFailed(nameof(ResetPasswordToken),
                    "Reset password token does not exist"));
            }

            return Result<ResetPasswordToken>.Success(resetPasswordToken);
        }
        catch (Exception e)
        {
            return Result<ResetPasswordToken>.Failure(EntityErrors.GetFailed(nameof(ResetPasswordToken),
                e.InnerException?.Message ?? "An unexpected error occurred while retrieving the reset password token"));
        }
    }

    public async Task<Result<ResetPasswordToken>> GetResetPasswordToken(string token)
    {
        try
        {
            var resetPasswordToken = await _context.ResetPasswordTokens.FirstOrDefaultAsync(t => t.Token == token);
            if (resetPasswordToken == null)
            {
                return Result<ResetPasswordToken>.Failure(EntityErrors.GetFailed(nameof(ResetPasswordToken),
                    "The given reset password token does not exist"));
            }

            return Result<ResetPasswordToken>.Success(resetPasswordToken);
        }
        catch (Exception e)
        {
            return Result<ResetPasswordToken>.Failure(EntityErrors.GetFailed(nameof(ResetPasswordToken),
                e.InnerException?.Message ?? "An unexpected error occurred while retrieving the reset password token"));
        }
    }

    public async Task<Result<Unit>> DeleteByUserIdAsync(Guid id)
    {
        try
        {
            var resetPasswordTokenResult = await GetByUserId(id);
            if (!resetPasswordTokenResult.IsSuccess)
            {
                return Result<Unit>.Failure(EntityErrors.NotFound(nameof(ResetPasswordToken),id));
            }
            
            var resetPasswordToken = resetPasswordTokenResult.Value!;
            _context.ResetPasswordTokens.Remove(resetPasswordToken);
            await _context.SaveChangesAsync();

            return Result<Unit>.Success(Unit.Value);
        }
        catch (Exception e)
        {
            return Result<Unit>.Failure(EntityErrors.DeleteFailed(nameof(ResetPasswordToken),e.InnerException?.Message ?? "An unexpected error occurred while deleting the location"));
        }
    }
}