namespace EmailSender.Domain.ValueObjects;

public sealed class RetryMetadata
{
    public int RetryAttempt { get; set; } = 0;
    public DateTime? LastTriedAt { get; set; }
    public string? LastError { get; set; }
}
