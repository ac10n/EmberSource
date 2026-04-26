using MailKit.Net.Smtp;
using MimeKit;

namespace Ember.WebServer.Areas.People.Services;

public class EmailSender(AuthSettings authSettings) : IEmailSender
{
    public async Task SendEmailAsync(string email, string subject, string htmlMessage)
    {
        var message = new MimeMessage();
        message.From.Add(new MailboxAddress("Ember", authSettings.Smtp.Username));
        message.To.Add(new MailboxAddress("", email));
        message.Subject = subject;

        var bodyBuilder = new BodyBuilder { HtmlBody = htmlMessage };
        message.Body = bodyBuilder.ToMessageBody();

        using var client = new SmtpClient();
        try
        {
            await client.ConnectAsync(authSettings.Smtp.Host, authSettings.Smtp.Port, authSettings.Smtp.EnableSsl);
            await client.AuthenticateAsync(authSettings.Smtp.Username, authSettings.Smtp.Password);
            await client.SendAsync(message);
        }
        finally
        {
            await client.DisconnectAsync(true);
        }
    }
}