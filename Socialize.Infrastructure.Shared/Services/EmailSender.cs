using Google.Apis.Auth.OAuth2;
using Google.Apis.Auth.OAuth2.Flows;
using Google.Apis.Auth.OAuth2.Responses;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;
using Socialize.Infrastructure.Shared.Services.Interfaces;
using Socialize.Infrastructure.Shared.Settings;

namespace Socialize.Infrastructure.Shared.Services
{
    public class EmailSender : IEmailSender
    {
        private readonly GoogleSettings _googleSettings;

        public EmailSender(IOptions<GoogleSettings> googleSettings)
        {
            _googleSettings = googleSettings.Value;
        }
        public async Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            var tokenResponse = await GetTokenAsync();
            var emailMessage = new MimeMessage();
            emailMessage.From.Add(new MailboxAddress(_googleSettings.SenderName, _googleSettings.SenderEmail));
            emailMessage.To.Add(new MailboxAddress("",email));
            emailMessage.Subject = subject;

            var bodyBuilder = new BodyBuilder { HtmlBody = htmlMessage };
            emailMessage.Body = bodyBuilder.ToMessageBody();

            using var client = new SmtpClient();
            try
            {
                await client.ConnectAsync("smtp.gmail.com", 587, SecureSocketOptions.StartTls);
                await client.AuthenticateAsync("oauth2", tokenResponse.AccessToken);
                await client.SendAsync(emailMessage);
            }
            finally
            {
                await client.DisconnectAsync(true);
            }

        }

        private async Task<TokenResponse> GetTokenAsync()
        {
            var secrets = new ClientSecrets
            {
                ClientId = _googleSettings.ClientId,
                ClientSecret = _googleSettings.ClientSecret
            };

            var token = new TokenResponse
            {
                RefreshToken = _googleSettings.RefreshToken
            };

            var flow = new GoogleAuthorizationCodeFlow(new GoogleAuthorizationCodeFlow.Initializer
            {
                ClientSecrets = secrets
            });

            var tokenResponse = await flow.RefreshTokenAsync("", token.RefreshToken, CancellationToken.None);

            return tokenResponse;
        }
    }
}
