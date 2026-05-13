namespace Ember.WebServer.Areas.People.Services;

public interface ISmsSender
{
    Task SendSmsAsync(string phoneNumber, string message);
}