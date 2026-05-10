namespace Ember.WebServer.Areas.People.Services;

public interface IInvitationNotificationService
{
    Task SendInvitationAsync(string realName, string inviteCode, string? email, string? phone);
}