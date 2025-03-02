using System.Net;
using System.Net.Mail;
using Microsoft.Extensions.Configuration;
using ModularMonolith.Modules.Customers.Application.Interfaces;
using ModularMonolith.Modules.Customers.Application.Messages;
using ModularMonolith.Shared.Abstractions.Messages;

namespace ModularMonolith.Modules.Customers.Infrastructure.Services;

public class EmailService : IEmailService
{
    private readonly IMessageBroker _messageBroker;

    public EmailService(IMessageBroker messageBroker)
    {
        _messageBroker = messageBroker;
    }

    public async Task SendEmailAsync(string to, string subject, string body)
    {
        var message = new SendEmailMessage
        {
            To = to,
            Subject = subject,
            Body = body
        };

        await _messageBroker.PublishAsync(message);
    }
}
