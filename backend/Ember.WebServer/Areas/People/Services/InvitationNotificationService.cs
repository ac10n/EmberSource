using System.Net;

namespace Ember.WebServer.Areas.People.Services;

public sealed class InvitationNotificationService(IEmailSender emailSender, ISmsSender smsSender) : IInvitationNotificationService
{
    public async Task SendInvitationAsync(string realName, string inviteCode, string? email, string? phone)
    {
        var subject = "Your Ember invitation code";
        var message = BuildMessage(realName, inviteCode);

        if (!string.IsNullOrWhiteSpace(email))
        {
            try
            {
                var htmlMessage = $"<p>{WebUtility.HtmlEncode(message)}</p>";
                await emailSender.SendEmailAsync(email, subject, htmlMessage);
            }
            catch
            {
                // Do not block invitation creation if email delivery fails.
            }
        }

        if (!string.IsNullOrWhiteSpace(phone))
        {
            try
            {
                await smsSender.SendSmsAsync(phone, message);
            }
            catch
            {
                // Do not block invitation creation if SMS delivery fails.
            }
        }
    }

    private static string BuildMessage(string realName, string inviteCode)
        => $"Hello {realName}, your Ember invitation code is {inviteCode}. Use it to register in the Ember app.";
}