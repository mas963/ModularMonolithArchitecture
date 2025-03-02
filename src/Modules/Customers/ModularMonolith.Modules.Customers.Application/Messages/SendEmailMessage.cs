using ModularMonolith.Shared.Abstractions.Messages;

namespace ModularMonolith.Modules.Customers.Application.Messages;

public record SendEmailMessage : IMessage
{
    public Guid Id { get; init; } = Guid.NewGuid();
    public DateTime Timestamp { get; init; } = DateTime.UtcNow;
    public string To { get; init; }
    public string Subject { get; init; }
    public string Body { get; init; }
    
}