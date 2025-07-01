namespace EmailSender.Core.Interfaces;

public interface IEmailConsumerService
{
    void Consume(string queueName, CancellationToken stoppingToken);
}
