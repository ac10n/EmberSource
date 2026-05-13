using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;
using Ember.Service;
using System.Diagnostics;

namespace Ember.WebServer.Areas.People.Services;

public class EmailSender(AuthSettings authSettings, ILogger<EmailSender> logger) : IEmailSender
{
    public async Task SendEmailAsync(string email, string subject, string htmlMessage)
    {
        var startedAt = Stopwatch.GetTimestamp();
        logger.LogInformation("Email send started. To={Email}, Subject={Subject}", email, subject);

        var message = new MimeMessage();
        var fromAddress = string.IsNullOrWhiteSpace(authSettings.Smtp.From)
            ? authSettings.Smtp.Username
            : authSettings.Smtp.From;
        message.From.Add(new MailboxAddress("Ember", fromAddress));
        message.To.Add(new MailboxAddress("", email));
        message.Subject = subject;

        var bodyBuilder = new BodyBuilder { HtmlBody = htmlMessage };
        message.Body = bodyBuilder.ToMessageBody();

        using var client = new SmtpClient();
        try
        {
            client.Timeout = 30_000;

            var socketOptions = authSettings.Smtp.EnableSsl
                ? authSettings.Smtp.Port == 465
                    ? SecureSocketOptions.SslOnConnect
                    : SecureSocketOptions.StartTls
                : SecureSocketOptions.None;

            await client.ConnectAsync(authSettings.Smtp.Host, authSettings.Smtp.Port, socketOptions);
            await client.AuthenticateAsync(authSettings.Smtp.Username, authSettings.Smtp.Password);
            await client.SendAsync(message);

            logger.LogInformation(
                "Email send succeeded. To={Email}, Subject={Subject}, ElapsedMs={ElapsedMs}",
                email,
                subject,
                Stopwatch.GetElapsedTime(startedAt).TotalMilliseconds);
        }
        catch (Exception ex)
        {
            logger.LogError(
                ex,
                "Email send failed. To={Email}, Subject={Subject}, ElapsedMs={ElapsedMs}",
                email,
                subject,
                Stopwatch.GetElapsedTime(startedAt).TotalMilliseconds);
            throw;
        }
        finally
        {
            logger.LogInformation(
                "Email send ended. To={Email}, Subject={Subject}, ElapsedMs={ElapsedMs}",
                email,
                subject,
                Stopwatch.GetElapsedTime(startedAt).TotalMilliseconds);

            try
            {
                await client.DisconnectAsync(true);
            }
            catch (Exception ex)
            {
                logger.LogWarning(ex, "Email client disconnect failed. To={Email}, Subject={Subject}", email, subject);
            }
        }
    }
}