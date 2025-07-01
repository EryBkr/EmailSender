namespace EmailSender.Shared.DTOs;

public sealed class DlqStatusDto
{
    public int RetriedCount { get; set; }
    public int SkippedCount { get; set; }
    public int TotalProcessed { get; set; }
}
