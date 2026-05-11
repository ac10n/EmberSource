using Ember.WebServer.Areas.People.Services;

namespace Ember.WebServer.Tests;

public sealed class InvitationNotificationServiceTests
{
    [Fact]
    public async Task SendInvitationAsync_ShouldStartEmailAndSmsConcurrently()
    {
        var emailSender = new BlockingEmailSender();
        var smsSender = new BlockingSmsSender();
        var service = new InvitationNotificationService(emailSender, smsSender);

        var sendTask = service.SendInvitationAsync("Test User", "INVITE123", "test@example.com", "+15555550123");

        await Task.WhenAll(
            emailSender.Started.Task.WaitAsync(TimeSpan.FromSeconds(1)),
            smsSender.Started.Task.WaitAsync(TimeSpan.FromSeconds(1)));

        Assert.False(sendTask.IsCompleted);

        emailSender.Release.TrySetResult(true);
        smsSender.Release.TrySetResult(true);

        await sendTask;
    }

    [Fact]
    public async Task SendInvitationAsync_ShouldSurfaceFailuresFromAnyNotification()
    {
        var emailSender = new FailingEmailSender();
        var smsSender = new CompletedSmsSender();
        var service = new InvitationNotificationService(emailSender, smsSender);

        await Assert.ThrowsAsync<InvalidOperationException>(
            () => service.SendInvitationAsync("Test User", "INVITE123", "test@example.com", "+15555550123"));
    }

    private sealed class BlockingEmailSender : IEmailSender
    {
        public TaskCompletionSource<bool> Started { get; } = new(TaskCreationOptions.RunContinuationsAsynchronously);
        public TaskCompletionSource<bool> Release { get; } = new(TaskCreationOptions.RunContinuationsAsynchronously);

        public Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            Started.TrySetResult(true);
            return Release.Task;
        }
    }

    private sealed class BlockingSmsSender : ISmsSender
    {
        public TaskCompletionSource<bool> Started { get; } = new(TaskCreationOptions.RunContinuationsAsynchronously);
        public TaskCompletionSource<bool> Release { get; } = new(TaskCreationOptions.RunContinuationsAsynchronously);

        public Task SendSmsAsync(string phoneNumber, string message)
        {
            Started.TrySetResult(true);
            return Release.Task;
        }
    }

    private sealed class FailingEmailSender : IEmailSender
    {
        public Task SendEmailAsync(string email, string subject, string htmlMessage)
            => Task.FromException(new InvalidOperationException("Email failed"));
    }

    private sealed class CompletedSmsSender : ISmsSender
    {
        public Task SendSmsAsync(string phoneNumber, string message)
            => Task.CompletedTask;
    }
}