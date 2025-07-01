using EmailSender.Core.Interfaces;

namespace EmailSender.Worker.Services;

public class EmailWorkerService : BackgroundService
{
    private readonly ILogger<EmailWorkerService> _logger;
    private readonly IEmailConsumerService _consumerService;

    public EmailWorkerService(ILogger<EmailWorkerService> logger, IEmailConsumerService consumerService)
    {
        _logger = logger;
        _consumerService = consumerService;
    }

    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("EmailWorkerService started");
        _consumerService.Consume("email_queue", stoppingToken);
        return Task.CompletedTask;
    }
}
