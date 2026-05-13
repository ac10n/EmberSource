using Ember.WebServer.Areas.People.Services;

namespace Ember.WebServer.Tests;

public sealed class DummySmsSender : ISmsSender
{
    public Task SendSmsAsync(string phoneNumber, string message)
    {
        return Task.CompletedTask;
    }
}