using DomainLayer.Common.Attributes;
using Microsoft.Extensions.Logging;
using System.Net.Mail;

namespace ApplicationLayer.Common.NotificationService
{
    [InjectAsScopedAttribute]
    public class EmailService(ILogger<EmailService> logger) : IEmailService
    {
        private readonly ILogger<EmailService> _logger = logger;

        public async Task<bool> SendEmailAsync(string to, string subject, string body)
        {
            using SmtpClient smtpClient = new();
            using MailMessage message = new();
            MailAddress fromAddress = new("no-reply@zboom.ir");

            smtpClient.Host = "10.1.10.53";
            smtpClient.Port = 2544;

            message.From = fromAddress;
            message.Subject = subject;
            message.IsBodyHtml = true;
            message.Body = body;
            message.To.Add(to);

            try
            {
                await smtpClient.SendMailAsync(message);
                return true;
            }
            catch (Exception exp)
            {
                _logger.LogError(exp.Message, "Send Email Error");
                return false;
            }
        }
    }
}