using EmailSender.Core.Interfaces;
using EmailSender.Domain.ValueObjects;
using EmailSender.Infrastructure.Mongo;
using EmailSender.Infrastructure.Providers;
using EmailSender.Infrastructure.Queue;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace EmailSender.Infrastructure.DI;

public static class ServiceRegistration
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<MongoSettings>(configuration.GetSection("MongoSettings"));
        services.AddSingleton<IEmailOutboxRepository, MongoEmailOutboxRepository>();
        services.Configure<RabbitMqSettings>(configuration.GetSection("RabbitMqSettings"));
        services.AddSingleton<IMessagePublisher, RabbitMqPublisher>();
        services.AddSingleton<IEmailConsumerService, RabbitMqConsumer>();
        services.Configure<SmtpSettings>(configuration.GetSection("SmtpSettings"));
        services.AddScoped<IEmailSenderProvider, MailKitEmailSenderProvider>();

        // diğer servisleri burada kaydedeceğiz
        return services;
    }
}
