using Application.Use_Cases.Commands.MailCommands;
using Application.Utils;
using Domain.Entities.User;
using Domain.Errors;
using Domain.Repositories;
using Domain.Services;
using Domain.Utils;
using MediatR;

namespace Application.Use_Cases.CommandHandlers.MailCommandHandlers;

public class ForgotPasswordCommandHandler : IRequestHandler<ForgotPasswordCommand, Result<string>>
{
    private readonly IUsersRepository _usersRepository;
    private readonly ITokenService _tokenService;
    private readonly IResetPasswordTokenRepository _resetPasswordTokenRepository;
    private readonly IMailService _mailService;

    public ForgotPasswordCommandHandler(IUsersRepository usersRepository, ITokenService tokenService,
        IResetPasswordTokenRepository resetPasswordTokenRepository, IMailService mailService)
    {
        _usersRepository = usersRepository;
        _tokenService = tokenService;
        _resetPasswordTokenRepository = resetPasswordTokenRepository;
        _mailService = mailService;
    }

    public async Task<Result<string>> Handle(ForgotPasswordCommand request, CancellationToken cancellationToken)
    {
        var user = await _usersRepository.GetByEmailAsync(request.Email, cancellationToken);
        if (!user.IsSuccess)
        {
            return Result<string>.Failure(AuthErrors.EmailNotFound(nameof(UserAuthentication), request.Email));
        }

        var resetPasswordToken = _tokenService.GenerateResetPasswordToken(user.Value!);
        await _resetPasswordTokenRepository.AddResetPasswordToken(resetPasswordToken);
       
        var emailContent = EmailContentBuilder.Build(user.Value!.FirstName, resetPasswordToken.Token);
        
        var emailResult = await _mailService.SendEmailAsync(
            receiver: request.Email,
            subject: "Reset password request",
            body: emailContent,
            cancellationToken: cancellationToken
        );

        if (!emailResult.IsSuccess)
        {
            return Result<string>.Failure(emailResult.Error!);
        }

        return Result<string>.Success(emailResult.Value!);
    }
}