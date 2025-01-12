using Domain.Utils;

namespace Domain.Services;

public interface IMailService
{
    Task<Result<string>> SendEmailAsync(string receiver, string subject, string body, CancellationToken cancellationToken);
}