namespace ModularMonolith.Shared.Abstractions.Messages;

public interface IMessage
{
    Guid Id { get; }
    DateTime Timestamp { get; }
}