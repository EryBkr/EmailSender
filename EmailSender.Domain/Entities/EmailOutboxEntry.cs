namespace EmailSender.Domain.Entities;

public sealed class EmailOutboxEntry
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public EmailMessage Message { get; set; } = null!;

    public bool IsPublished { get; set; } = false;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? PublishedAt { get; set; }
}
