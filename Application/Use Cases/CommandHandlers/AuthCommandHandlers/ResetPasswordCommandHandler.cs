using Application.DTOs;
using Application.Use_Cases.Commands.AuthCommands;
using AutoMapper;
using Domain.Entities.User;
using Domain.Errors;
using Domain.Repositories;
using Domain.Services;
using Domain.Utils;
using MediatR;

namespace Application.Use_Cases.CommandHandlers.AuthCommandHandlers;

public class ResetPasswordCommandHandler : IRequestHandler<ResetPasswordCommand, Result<string>>
{
    private readonly IUsersRepository _usersRepository;
    private readonly ITokenService _tokenService;
    private readonly IPasswordHashingService _passwordHashingService;
    private readonly IResetPasswordTokenRepository _resetPasswordTokenRepository;

    public ResetPasswordCommandHandler(IUsersRepository usersRepository, ITokenService tokenService, 
        IMapper mapper, IPasswordHashingService passwordHashingService, IResetPasswordTokenRepository resetPasswordTokenRepository)
    {
        _usersRepository = usersRepository;
        _tokenService = tokenService;
        _passwordHashingService = passwordHashingService;
        _resetPasswordTokenRepository = resetPasswordTokenRepository;
    }

    public async Task<Result<string>> Handle(ResetPasswordCommand request, CancellationToken cancellationToken)
    {
        var resetPasswordToken = await _tokenService.ValidateResetPasswordTokenAsync(request.Token);
        if (resetPasswordToken == null)
        {
            return Result<string>.Failure(
                AuthErrors.InvalidResetPasswordToken(nameof(ResetPasswordToken), "Invalid reset password token"));
        }

        var user = await _usersRepository.GetByIdAsync(resetPasswordToken.UserId, cancellationToken);
        if (!user.IsSuccess)
        {
            return Result<string>.Failure(user.Error!);
        }
        
        var deleteTokenResult = await _resetPasswordTokenRepository.DeleteByUserIdAsync(resetPasswordToken.UserId);
        if (!deleteTokenResult.IsSuccess)
        {
            return Result<string>.Failure(deleteTokenResult.Error!);
        }
        
        var hashedPassword = _passwordHashingService.HashPassword(request.Password);
        user.Value!.Password = hashedPassword;
        var userResult = await _usersRepository.UpdateUserPasswordAsync(user.Value!, cancellationToken);
        
        return !userResult.IsSuccess
            ? Result<string>.Failure(userResult.Error!)
            : Result<string>.Success("Password successfully reset");
    }
}