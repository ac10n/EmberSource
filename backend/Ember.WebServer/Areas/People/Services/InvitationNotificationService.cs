using System.Net;
using System.Collections.Generic;

namespace Ember.WebServer.Areas.People.Services;

public sealed class InvitationNotificationService(IEmailSender emailSender, ISmsSender smsSender) : IInvitationNotificationService
{
    public async Task SendInvitationAsync(string realName, string inviteCode, string? email, string? phone)
    {
        var subject = "Your Ember invitation code";
        var message = BuildMessage(realName, inviteCode);
        var sendTasks = new List<Task>(capacity: 2);

        if (!string.IsNullOrWhiteSpace(email))
        {
            var htmlMessage = $"<p>{WebUtility.HtmlEncode(message)}</p>";
            sendTasks.Add(emailSender.SendEmailAsync(email, subject, htmlMessage));
        }

        if (!string.IsNullOrWhiteSpace(phone))
        {
            sendTasks.Add(smsSender.SendSmsAsync(phone, message));
        }

        if (sendTasks.Count == 0)
        {
            return;
        }

        await Task.WhenAll(sendTasks);
    }

    private static string BuildMessage(string realName, string inviteCode)
        => $"Hello {realName}, your Ember invitation code is {inviteCode}. Use it to register in the Ember app.";
}