using System.Text.Json;
using Microsoft.Extensions.Configuration;
using ModularMonolith.Shared.Abstractions.Messages;
using RabbitMQ.Client;

namespace ModularMonolith.Shared.Infrastructure.Messages.RabbitMQ;

public class RabbitMQMessageBroker : IMessageBroker
{
    private readonly IConnection _connection;
    private readonly IModel _channel;
    private const string ExchangeName = "ecommerce_exchange";

    public RabbitMQMessageBroker(IConfiguration configuration)
    {
        var factory = new ConnectionFactory
        {
            HostName = configuration["RabbitMQ:HostName"],
            UserName = configuration["RabbitMQ:Username"],
            Password = configuration["RabbitMQ:Password"]
        };

        _connection = factory.CreateConnection();
        _channel = _connection.CreateModel();
        _channel.ExchangeDeclare(ExchangeName, ExchangeType.Direct, true);
    }

    public async Task PublishAsync<T>(T message) where T : class, IMessage
    {
        var routingKey = typeof(T).Name;
        var body = JsonSerializer.SerializeToUtf8Bytes(message);

        _channel.BasicPublish(
            exchange: ExchangeName,
            routingKey: routingKey,
            basicProperties: null,
            body: body);

        await Task.CompletedTask;
    }
}