using EmailSender.Core.Interfaces;
using EmailSender.Domain.Entities;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json;

namespace EmailSender.Infrastructure.Queue;

public class RabbitMqConsumer : IEmailConsumerService
{
    private readonly RabbitMqSettings _settings;
    private readonly IModel _channel;
    private readonly IConnection _connection;
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<RabbitMqConsumer> _logger;

    public RabbitMqConsumer(
    IOptions<RabbitMqSettings> options,
    IServiceProvider serviceProvider,
    ILogger<RabbitMqConsumer> logger)
    {
        _settings = options.Value;
        _serviceProvider = serviceProvider;
        _logger = logger;

        var factory = new ConnectionFactory()
        {
            HostName = _settings.HostName,
            Port = _settings.Port,
            UserName = _settings.Username,
            Password = _settings.Password,
            DispatchConsumersAsync = true
        };

        _connection = factory.CreateConnection();
        _channel = _connection.CreateModel();
    }

    public void Consume(string queueName, CancellationToken stoppingToken)
    {
        var consumer = new AsyncEventingBasicConsumer(_channel);

        consumer.Received += async (model, ea) =>
        {
            try
            {
                var body = ea.Body.ToArray();
                var json = Encoding.UTF8.GetString(body);
                var message = JsonSerializer.Deserialize<EmailMessage>(json);

                if (message is null)
                {
                    _logger.LogWarning("Message could not be deserialized");
                    return;
                }

                using var scope = _serviceProvider.CreateScope();
                var provider = scope.ServiceProvider.GetRequiredService<IEmailSenderProvider>();

                await provider.SendEmailAsync(message, stoppingToken);

                _channel.BasicAck(ea.DeliveryTag, multiple: false);
                _logger.LogInformation("Email sent to {To}", message.To);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while consuming message");
                _channel.BasicNack(ea.DeliveryTag, false, requeue: false); // DLQ için önemli
            }
        };

        _channel.BasicConsume(queue: queueName, autoAck: false, consumer: consumer);
    }
}
