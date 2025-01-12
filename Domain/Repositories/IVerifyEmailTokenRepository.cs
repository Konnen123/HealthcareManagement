using Domain.Entities.Tokens;
using Domain.Utils;
using MediatR;

namespace Domain.Repositories;

public interface IVerifyEmailTokenRepository
{
    Task<Result<Unit>> AddVerifyEmailTokenAsync(VerifyEmailToken token);
    Task<Result<VerifyEmailToken>> GetVerifyEmailTokenAsync(string token);
    Task<Result<Unit>> DeleteByUserIdAsync(Guid id);
}