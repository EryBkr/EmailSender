namespace EmailSender.Shared.DTOs;

public sealed class EmailMessageDto
{
    public string To { get; set; } = null!;
    public string Subject { get; set; } = null!;
    public string Body { get; set; } = null!;
    public string? PreferredProvider { get; set; } // "MailKit", "SendGrid"
}
