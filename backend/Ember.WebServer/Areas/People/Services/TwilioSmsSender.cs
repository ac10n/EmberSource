using System.Net.Http.Headers;
using System.Text;
using Ember.Service;

namespace Ember.WebServer.Areas.People.Services;

public sealed class TwilioSmsSender(IHttpClientFactory httpClientFactory, AuthSettings authSettings) : ISmsSender
{
    public async Task SendSmsAsync(string phoneNumber, string message)
    {
        if (string.IsNullOrWhiteSpace(authSettings.Twilio.AccountSid)
            || string.IsNullOrWhiteSpace(authSettings.Twilio.AuthToken)
            || string.IsNullOrWhiteSpace(authSettings.Twilio.FromNumber))
        {
            return;
        }

        var client = httpClientFactory.CreateClient();
        var endpoint = $"https://api.twilio.com/2010-04-01/Accounts/{authSettings.Twilio.AccountSid}/Messages.json";
        var request = new HttpRequestMessage(HttpMethod.Post, endpoint)
        {
            Content = new FormUrlEncodedContent(new Dictionary<string, string>
            {
                ["To"] = phoneNumber,
                ["From"] = authSettings.Twilio.FromNumber,
                ["Body"] = message,
            })
        };

        var credentials = Convert.ToBase64String(Encoding.ASCII.GetBytes($"{authSettings.Twilio.AccountSid}:{authSettings.Twilio.AuthToken}"));
        request.Headers.Authorization = new AuthenticationHeaderValue("Basic", credentials);

        using var response = await client.SendAsync(request);
        response.EnsureSuccessStatusCode();
    }
}