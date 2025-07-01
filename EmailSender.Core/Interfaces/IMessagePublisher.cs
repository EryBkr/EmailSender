namespace EmailSender.Core.Interfaces;

public interface IMessagePublisher
{
    Task PublishAsync<T>(T message, string routingKey) where T : class;
}
