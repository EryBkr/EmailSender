using EmailSender.Domain.Enums;
using EmailSender.Domain.ValueObjects;

namespace EmailSender.Domain.Entities;

public sealed class EmailMessage
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public string To { get; set; } = null!;
    public string Subject { get; set; } = null!;
    public string Body { get; set; } = null!;

    public EmailProviderType? PreferredProvider { get; set; }

    public EmailStatus Status { get; set; } = EmailStatus.Queued;
    public RetryMetadata RetryMetadata { get; set; } = new();

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? SentAt { get; set; }
}
