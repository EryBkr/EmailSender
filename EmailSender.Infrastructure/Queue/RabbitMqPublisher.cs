using System.Text;
using System.Text.Json;
using EmailSender.Core.Interfaces;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;

namespace EmailSender.Infrastructure.Queue;

public sealed class RabbitMqPublisher : IMessagePublisher, IDisposable
{
    private readonly RabbitMqSettings _settings;
    private readonly IConnection _connection;
    private readonly IModel _channel;

    public RabbitMqPublisher(IOptions<RabbitMqSettings> options)
    {
        _settings = options.Value;
        var factory = new ConnectionFactory
        {
            HostName = _settings.HostName,
            Port = _settings.Port,
            UserName = _settings.Username,
            Password = _settings.Password
        };
        _connection = factory.CreateConnection();
        _channel = _connection.CreateModel();

        _channel.ExchangeDeclare(_settings.Exchange, ExchangeType.Direct, durable: true);
        _channel.QueueDeclare(_settings.Queue, durable: true, exclusive: false, autoDelete: false);
        _channel.QueueBind(_settings.Queue, _settings.Exchange, routingKey: _settings.Queue);
    }

    public Task PublishAsync<T>(T message, string routingKey) where T : class
    {
        var body = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(message));
        _channel.BasicPublish(exchange: _settings.Exchange,
                              routingKey: routingKey,
                              basicProperties: null,
                              body: body);

        return Task.CompletedTask;
    }

    public void Dispose()
    {
        _channel?.Close();
        _channel?.Dispose();
        _connection?.Close();
        _connection?.Dispose();
    }
}
