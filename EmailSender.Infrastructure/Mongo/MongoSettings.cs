namespace EmailSender.Infrastructure.Mongo;

public class MongoSettings
{
    public string ConnectionString { get; set; } = null!;
    public string DatabaseName { get; set; } = null!;
    public string OutboxCollectionName { get; set; } = "EmailOutbox";
}
