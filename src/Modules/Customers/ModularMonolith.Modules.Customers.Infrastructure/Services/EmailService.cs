using System.Net;
using System.Net.Mail;
using Microsoft.Extensions.Configuration;
using ModularMonolith.Modules.Customers.Application.Interfaces;

namespace ModularMonolith.Modules.Customers.Infrastructure.Services;

public class EmailService : IEmailService
{
    private readonly IConfiguration _configuration;
    
    public EmailService(IConfiguration configuration)
    {
        _configuration = configuration;
    }
    
    public async Task SendEmailAsync(string to, string subject, string body)
    {
        var emailSetting = _configuration.GetSection("EmailSettings");

        var fromAddress = new MailAddress(emailSetting["From"], emailSetting["DisplayName"]);

        var toAddress = new MailAddress(to);

        var smtp = new SmtpClient
        {
            Host = emailSetting["Host"],
            Port = int.Parse(emailSetting["Port"]),
            EnableSsl = true,
            DeliveryMethod = SmtpDeliveryMethod.Network,
            UseDefaultCredentials = false,
            Credentials = new NetworkCredential(emailSetting["Username"], emailSetting["Password"])
        };

        using (var message = new MailMessage(fromAddress, toAddress))
        {
            message.Subject = subject;
            message.Body = body;
            message.IsBodyHtml = true;

            await smtp.SendMailAsync(message);
        }
    }
}
