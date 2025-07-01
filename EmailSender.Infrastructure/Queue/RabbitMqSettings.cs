namespace EmailSender.Infrastructure.Queue;

public sealed class RabbitMqSettings
{
    public string HostName { get; set; } = null!;
    public int Port { get; set; } = 5672;
    public string Username { get; set; } = "guest";
    public string Password { get; set; } = "guest";
    public string Exchange { get; set; } = "email_exchange";
    public string Queue { get; set; } = "email_queue";
}
