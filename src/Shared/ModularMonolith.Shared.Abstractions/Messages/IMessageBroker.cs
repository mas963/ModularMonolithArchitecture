namespace ModularMonolith.Shared.Abstractions.Messages;

public interface IMessageBroker
{
    Task PublishAsync<T>(T message) where T : class, IMessage;
}
