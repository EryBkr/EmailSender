using EmailSender.Domain.Entities;

namespace EmailSender.Core.Interfaces;

public interface IEmailSenderProvider
{
    Task SendEmailAsync(EmailMessage message, CancellationToken cancellationToken = default);
}
