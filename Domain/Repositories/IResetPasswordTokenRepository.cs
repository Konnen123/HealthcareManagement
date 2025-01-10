using Domain.Entities.User;
using Domain.Utils;
using MediatR;

namespace Domain.Repositories;

public interface IResetPasswordTokenRepository
{
    Task<Result<Unit>> AddResetPasswordToken(ResetPasswordToken token);
    Task<Result<ResetPasswordToken>> GetByUserId(Guid userId);
    Task<Result<ResetPasswordToken>> GetResetPasswordToken(string token);
    Task<Result<Unit>> DeleteByUserIdAsync(Guid id);
}