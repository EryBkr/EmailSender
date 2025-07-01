namespace EmailSender.Domain.Enums;

public enum EmailStatus
{
    Queued = 0,
    Sent = 1,
    Failed = 2,
    Retrying = 3,
    DeadLettered = 4
}
