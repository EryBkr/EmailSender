using EmailSender.Core.Interfaces;
using EmailSender.Domain.Entities;
using EmailSender.Domain.ValueObjects;
using MailKit.Net.Smtp;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MimeKit;
namespace EmailSender.Infrastructure.Providers;

public class MailKitEmailSenderProvider : IEmailSenderProvider
{
    private readonly ILogger<MailKitEmailSenderProvider> _logger;
    private readonly SmtpSettings _smtpSettings;

    public MailKitEmailSenderProvider(IOptions<SmtpSettings> smtpOptions, ILogger<MailKitEmailSenderProvider> logger)
    {
        _smtpSettings = smtpOptions.Value;
        _logger = logger;
    }

    public async Task SendEmailAsync(EmailMessage message, CancellationToken cancellationToken = default)
    {
        var email = new MimeMessage();
        email.From.Add(MailboxAddress.Parse(_smtpSettings.From));
        email.To.Add(MailboxAddress.Parse(message.To));
        email.Subject = message.Subject;

        var builder = new BodyBuilder { HtmlBody = message.Body };
        email.Body = builder.ToMessageBody();

        using var smtp = new SmtpClient();
        await smtp.ConnectAsync(_smtpSettings.Host, _smtpSettings.Port, _smtpSettings.UseSsl, cancellationToken);
        await smtp.AuthenticateAsync(_smtpSettings.Username, _smtpSettings.Password, cancellationToken);
        await smtp.SendAsync(email, cancellationToken);
        await smtp.DisconnectAsync(true, cancellationToken);

        _logger.LogInformation("Email sent to {To}", message.To);
    }
}
