using EmailSender.Domain.Entities;

namespace EmailSender.Infrastructure.Mongo;

public interface IEmailOutboxRepository
{
    Task AddAsync(EmailOutboxEntry entry);
    Task<List<EmailOutboxEntry>> GetUnpublishedAsync(int batchSize = 10);
    Task MarkAsPublishedAsync(string id);
}
