using EmailSender.Infrastructure.DI;
using EmailSender.Worker;
using EmailSender.Worker.Services;

var builder = Host.CreateApplicationBuilder(args);
builder.Services.AddInfrastructure(builder.Configuration);

builder.Services.AddHostedService<Worker>();
builder.Services.AddHostedService<OutboxPublisherWorker>();

var host = builder.Build();
host.Run();
