using EmailSender.Domain.Entities;
using EmailSender.Domain.Enums;
using EmailSender.Infrastructure.Mongo;
using EmailSender.Shared.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace EmailSender.API.Controllers;

[ApiController]
[Route("[controller]")]
public class EmailController: ControllerBase
{
    private readonly IEmailOutboxRepository _outboxRepository;
    private readonly ILogger<EmailController> _logger;

    public EmailController(IEmailOutboxRepository outboxRepository, ILogger<EmailController> logger)
    {
        _outboxRepository = outboxRepository;
        _logger = logger;
    }

    [HttpPost("send")]
    public async Task<IActionResult> SendEmail([FromBody] EmailMessageDto request)
    {
        if (!ModelState.IsValid)
            return BadRequest("Invalid request");

        var message = new EmailMessage
        {
            To = request.To,
            Subject = request.Subject,
            Body = request.Body,
            PreferredProvider = Enum.TryParse<EmailProviderType>(request.PreferredProvider, true, out var parsed)
                ? parsed
                : null
        };

        var outboxEntry = new EmailOutboxEntry
        {
            Message = message
        };

        await _outboxRepository.AddAsync(outboxEntry);

        _logger.LogInformation("Email message queued to outbox: {EmailId}", outboxEntry.Id);

        return Ok(new EmailResponseDto
        {
            Success = true,
            Message = "Queued to Outbox",
            EmailId = outboxEntry.Id
        });
    }
}
