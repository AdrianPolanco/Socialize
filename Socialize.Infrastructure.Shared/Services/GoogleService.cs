using Google.Apis.Auth.OAuth2;
using Google.Apis.Gmail.v1;
using Google.Apis.Util.Store;
using Microsoft.Extensions.Options;
using Socialize.Infrastructure.Shared.Settings;


namespace Socialize.Infrastructure.Shared.Services
{
    public class GoogleService
    {
        private readonly GoogleSettings _googleSettings;

        public GoogleService(IOptions<GoogleSettings> googleSettings)
        {
            _googleSettings = googleSettings.Value;
        }

        public async Task<UserCredential> GetUserCredentialAsync()
        {
            using var stream = new FileStream("client_secret.json", FileMode.Open, FileAccess.Read);
            var credPath = "token.json";

            var credential = await GoogleWebAuthorizationBroker.AuthorizeAsync(
                GoogleClientSecrets.Load(stream).Secrets,
                new[] { GmailService.Scope.GmailSend },
                "user",
                CancellationToken.None,
                new FileDataStore(credPath, true));

            return credential;
        }
    }

}
