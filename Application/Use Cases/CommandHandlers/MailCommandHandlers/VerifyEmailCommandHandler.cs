using Application.Use_Cases.Commands.MailCommands;
using Application.Utils;
using Domain.Entities.Tokens;
using Domain.Entities.User;
using Domain.Errors;
using Domain.Repositories;
using Domain.Services;
using Domain.Utils;
using MediatR;

namespace Application.Use_Cases.CommandHandlers.MailCommandHandlers;

public class VerifyEmailCommandHandler : IRequestHandler<VerifyEmailCommand, Result<string>>
{
    private readonly IMailService _mailService;
    private readonly ITokenService _tokenService;
    private readonly IUsersRepository _usersRepository;
    private readonly IVerifyEmailTokenRepository _verifyEmailTokenRepository;

    public VerifyEmailCommandHandler(IMailService mailService, ITokenService tokenService, IUsersRepository usersRepository, IVerifyEmailTokenRepository verifyEmailTokenRepository)
    {
        _mailService = mailService;
        _tokenService = tokenService;
        _usersRepository = usersRepository;
        _verifyEmailTokenRepository = verifyEmailTokenRepository;
    }

    public async Task<Result<string>> Handle(VerifyEmailCommand request, CancellationToken cancellationToken)
    {
        var user = await _usersRepository.GetByEmailAsync(request.Email, cancellationToken);
        if (!user.IsSuccess)
        {
            return Result<string>.Failure(AuthErrors.EmailNotFound(nameof(User), request.Email));
        }

        var emailToken = _tokenService.GenerateVerifyEmailToken(user.Value!);
        await _verifyEmailTokenRepository.AddVerifyEmailTokenAsync(emailToken);
        var verifyEmailContent = EmailContentBuilder.BuildVerificationEmail(user.Value!.FirstName, emailToken.Token);
        var emailResult = await _mailService.SendEmailAsync(
            receiver: request.Email,
            subject: "Verify your email",
            body: verifyEmailContent,
            cancellationToken: cancellationToken
        );

        if (!emailResult.IsSuccess)
        {
            return Result<string>.Failure(emailResult.Error!);
        }

        return Result<string>.Success(emailResult.Value!);
    }
}