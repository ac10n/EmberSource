using Ember.WebServer.Areas.People.Services;

namespace Ember.WebServer.Tests;

public class DummyEmailSender : IEmailSender
{
    public Task SendEmailAsync(string email, string subject, string htmlMessage)
    {
        // Do nothing for tests
        return Task.CompletedTask;
    }
}