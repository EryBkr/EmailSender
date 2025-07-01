
using EmailSender.Core.Interfaces;
using EmailSender.Infrastructure.Mongo;
using EmailSender.Infrastructure.Queue;
using Microsoft.Extensions.Options;

namespace EmailSender.Worker.Services;

public class OutboxPublisherWorker : BackgroundService
{
    private readonly ILogger<OutboxPublisherWorker> _logger;
    private readonly IEmailOutboxRepository _outboxRepository;
    private readonly IMessagePublisher _publisher;
    private readonly RabbitMqSettings _settings;

    public OutboxPublisherWorker(
       ILogger<OutboxPublisherWorker> logger,
       IEmailOutboxRepository outboxRepository,
       IMessagePublisher publisher,
       IOptions<RabbitMqSettings> options)
    {
        _logger = logger;
        _outboxRepository = outboxRepository;
        _publisher = publisher;
        _settings = options.Value;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            var entries = await _outboxRepository.GetUnpublishedAsync();

            foreach (var entry in entries)
            {
                await _publisher.PublishAsync(entry.Message, _settings.Queue);

                
                await _outboxRepository.MarkAsPublishedAsync(entry.Id);

                _logger.LogInformation("Published and marked as published: {EmailId}", entry.Id);
            }

            await Task.Delay(TimeSpan.FromSeconds(5), stoppingToken);
        }
    }
}
