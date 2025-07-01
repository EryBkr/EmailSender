using EmailSender.Domain.Entities;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace EmailSender.Infrastructure.Mongo;

public class MongoEmailOutboxRepository: IEmailOutboxRepository
{
    private readonly IMongoCollection<EmailOutboxEntry> _collection;

    public MongoEmailOutboxRepository(IOptions<MongoSettings> options)
    {
        var client = new MongoClient(options.Value.ConnectionString);
        var database = client.GetDatabase(options.Value.DatabaseName);
        _collection = database.GetCollection<EmailOutboxEntry>(options.Value.OutboxCollectionName);
    }

    public async Task AddAsync(EmailOutboxEntry entry) 
        => await _collection.InsertOneAsync(entry);

    public async Task<List<EmailOutboxEntry>> GetUnpublishedAsync(int batchSize = 10)
        => await _collection.Find(x => !x.IsPublished)
                                .SortBy(x => x.CreatedAt)
                                .Limit(batchSize)
                                .ToListAsync();

    public async Task MarkAsPublishedAsync(string id)
    {
        var update = Builders<EmailOutboxEntry>.Update
            .Set(x => x.IsPublished, true)
            .Set(x => x.PublishedAt, DateTime.UtcNow);

        await _collection.UpdateOneAsync(x => x.Id == id, update);
    }
}
