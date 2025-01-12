using System.Net;
using System.Net.Mail;
using Application.Utils;
using Domain.Errors;
using Domain.Services;
using Domain.Utils;
using Microsoft.Extensions.Options;

namespace Identity.Services;

public class SmtpEmailService : IMailService
{
    private readonly SmtpSettings _smtpSettings;

    public SmtpEmailService(IOptions<SmtpSettings> smtpSettings)
    {
        _smtpSettings = smtpSettings.Value;
    }

    public async Task<Result<string>> SendEmailAsync(string receiver, string subject, string body, CancellationToken cancellationToken)
    {
        try
        {
            var smtpClient = new SmtpClient(_smtpSettings.SmtpServer)
            {
                Port = _smtpSettings.Port,
                Credentials = new NetworkCredential(_smtpSettings.Username, _smtpSettings.Password),
                EnableSsl = true 
            };
            var mailMessage = new MailMessage
            {
                From = new MailAddress(_smtpSettings.FromEmail),
                Subject = subject,
                Body = body,
                IsBodyHtml = true
            };
            mailMessage.To.Add(receiver);
            await smtpClient.SendMailAsync(mailMessage, cancellationToken);

            return Result<string>.Success("Email sent successfully.");
        }
        catch (Exception ex)
        {
            return Result<string>.Failure(SendMailErrors.EmailNotSent(nameof(SmtpEmailService), "Failed to send email."));
        }
    }
}