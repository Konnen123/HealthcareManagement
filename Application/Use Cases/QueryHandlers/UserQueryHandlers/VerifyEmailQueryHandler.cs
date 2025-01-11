using Application.Use_Cases.Queries.UserQueries;
using Domain.Entities.Tokens;
using Domain.Entities.User;
using Domain.Errors;
using Domain.Repositories;
using Domain.Services;
using Domain.Utils;
using MediatR;

namespace Application.Use_Cases.QueryHandlers.UserQueryHandlers;

public class VerifyEmailQueryHandler : IRequestHandler<VerifyEmailQuery, Result<string>>
{
    private readonly IVerifyEmailTokenRepository _verifyEmailTokenRepository;
    private readonly IUsersRepository _usersRepository;

    public VerifyEmailQueryHandler(IVerifyEmailTokenRepository verifyEmailTokenRepository, IUsersRepository usersRepository)
    {
        _verifyEmailTokenRepository = verifyEmailTokenRepository;
        _usersRepository = usersRepository;
    }
    public async Task<Result<string>> Handle(VerifyEmailQuery request, CancellationToken cancellationToken)
    {
        Console.WriteLine(request.Token);
        var foundToken = await _verifyEmailTokenRepository.GetVerifyEmailTokenAsync(request.Token);
        if (!foundToken.IsSuccess)
        {
            return Result<string>.Failure(EntityErrors.GetFailed(nameof(VerifyEmailToken), "verify-email?status=error&description=token_not_found"));
        }
        
        
        if(foundToken.Value!.ExpiresAt < DateTime.UtcNow)
        {
            return Result<string>.Failure(EntityErrors.GetFailed(nameof(VerifyEmailToken), "verify-email?status=error&description=token_expired"));
        }

        var user = await _usersRepository.GetByIdAsync(foundToken.Value.UserId, cancellationToken);
        if (!user.IsSuccess)
        {
            return Result<string>.Failure(EntityErrors.GetFailed(nameof(User), "verify-email?status=error&description=user_not_found"));
        }

        
        var resultToVerificationInDb = await _usersRepository.VerifyEmailAsync(user.Value!, cancellationToken);
        if (!resultToVerificationInDb.IsSuccess)
        {
            return Result<string>.Failure(EntityErrors.GetFailed(nameof(User), "verify-email?status=error&description=verification_failed"));
        }
        var resultToEmailTokenDeletion = await _verifyEmailTokenRepository.DeleteByUserIdAsync(user.Value!.UserId);
        if (!resultToEmailTokenDeletion.IsSuccess)
        {
                return Result<string>.Failure(EntityErrors.GetFailed(nameof(VerifyEmailToken), "verify-email?status=error&description=token_deletion_failed"));
        }
        return Result<string>.Success("verify-email?status=success");
    }
}