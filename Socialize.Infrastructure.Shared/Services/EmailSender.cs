
using MailKit.Net.Smtp;
using Microsoft.Extensions.Options;
using MimeKit;
using Socialize.Infrastructure.Shared.Services.Interfaces;
using Socialize.Infrastructure.Shared.Settings;

namespace Socialize.Infrastructure.Shared.Services
{
    public class EmailSender : IEmailSender
    {
        private readonly SmtpSettings _smtpSettings;

        public EmailSender(IOptions<SmtpSettings> smtpSettings)
        {
            _smtpSettings = smtpSettings.Value;
        }
        public async Task SendEmailAsync(string email, string subject, string message)
        {

            MimeMessage emailMessage = new();

            emailMessage.From.Add(new MailboxAddress(_smtpSettings.SenderName, _smtpSettings.SenderEmail));
            emailMessage.To.Add(new MailboxAddress("", email));
            emailMessage.Subject = subject;

            BodyBuilder bodyBuilder = new() { HtmlBody = message };
            emailMessage.Body = bodyBuilder.ToMessageBody();

            using SmtpClient client = new();

            await client.ConnectAsync(_smtpSettings.Server, _smtpSettings.Port, true);
            await client.AuthenticateAsync(_smtpSettings.UserName, _smtpSettings.Password);
            await client.SendAsync(emailMessage);
            await client.DisconnectAsync(true);
        }
    }
}
