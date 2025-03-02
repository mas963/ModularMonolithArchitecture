using System.Net;
using System.Net.Mail;
using System.Text.Json;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using ModularMonolith.Modules.Customers.Application.Messages;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace ModularMonolith.Modules.Customers.Infrastructure.BackgroundServices;

public class EmailProcessor : BackgroundService
{
    private readonly IConnection _connection;
    private readonly IModel _channel;
    private readonly ILogger<EmailProcessor> _logger;
    private const string ExchangeName = "ecommerce_exchange";
    private const string QueueName = "email_queue";
    private readonly IConfigurationSection _emailSettings;

    public EmailProcessor(IConfiguration configuration, ILogger<EmailProcessor> logger)
    {
        var factory = new ConnectionFactory
        {
            HostName = configuration["RabbitMQ:Host"],
            UserName = configuration["RabbitMQ:Username"],
            Password = configuration["RabbitMQ:Password"]
        };

        _connection = factory.CreateConnection();
        _channel = _connection.CreateModel();
        _logger = logger;

        _channel.ExchangeDeclare(ExchangeName, ExchangeType.Direct, durable: true);
        _channel.QueueDeclare(QueueName, durable: true, exclusive: false, autoDelete: false);
        _channel.QueueBind(QueueName, ExchangeName, typeof(SendEmailMessage).Name);

        _emailSettings = configuration.GetSection("EmailSettings");
    }

    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var consumer = new EventingBasicConsumer(_channel);

        consumer.Received += async (model, ea) =>
        {
            try
            {
                var body = ea.Body.ToArray();
                var message = JsonSerializer.Deserialize<SendEmailMessage>(body);

                // implement actual email sending logic here
                await SendEmailImplementation(message);

                _channel.BasicAck(ea.DeliveryTag, false);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error processing email message");
                _channel.BasicNack(ea.DeliveryTag, false, true);
            }
        };

        _channel.BasicConsume(queue: QueueName, autoAck: false, consumer: consumer);

        return Task.CompletedTask;
    }

    private async Task SendEmailImplementation(SendEmailMessage sendEmailMessage)
    {
        var fromAddress = new MailAddress(_emailSettings["From"], _emailSettings["DisplayName"]);

        var toAddress = new MailAddress(sendEmailMessage.To);

        var smtp = new SmtpClient
        {
            Host = _emailSettings["Host"],
            Port = int.Parse(_emailSettings["Port"]),
            EnableSsl = true,
            DeliveryMethod = SmtpDeliveryMethod.Network,
            UseDefaultCredentials = false,
            Credentials = new NetworkCredential(_emailSettings["Username"], _emailSettings["Password"])
        };

        using (var message = new MailMessage(fromAddress, toAddress))
        {
            message.Subject = sendEmailMessage.Subject;
            message.Body = sendEmailMessage.Body;
            message.IsBodyHtml = true;

            await smtp.SendMailAsync(message);
        }
    }

    public override void Dispose()
    {
        _channel?.Dispose();
        _connection?.Dispose();
        base.Dispose();
    }
}