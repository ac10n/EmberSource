using System.Security.Claims;
using System.Diagnostics;
using Ember.Domain.EmberEntities;
using Ember.Infrastructure;
using Ember.Service;
using Ember.WebServer.Areas.People.Controllers;
using Ember.WebServer.Areas.People.Services;
using Ember.WebServer.Helpers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Ember.WebServer.Tests;

public sealed class InvitationControllerTests
{
    [Fact]
    public async Task CreateInvitation_ShouldReturnBadRequest_WhenNoContactMethodIsProvided()
    {
        using var db = CreateDbContext();
        var notificationService = new RecordingInvitationNotificationService();
        var controller = CreateController(db, notificationService);

        var result = await controller.CreateInvitation(new CreateInvitationDto(
            "Test User",
            true,
            "Canada",
            null,
            null,
            DateTimeOffset.UtcNow.AddDays(7)));

        Assert.IsType<BadRequestObjectResult>(result);
        Assert.False(notificationService.WasCalled);
        Assert.Empty(await db.Invitations.ToListAsync());
    }

    [Fact]
    public async Task CreateInvitation_ShouldReturnBeforeNotificationCompletes_WhenEmailIsProvided()
    {
        using var db = CreateDbContext();
        var notificationService = new RecordingInvitationNotificationService();
        var controller = CreateController(db, notificationService);

        var actionTask = controller.CreateInvitation(new CreateInvitationDto(
            "Test User",
            true,
            "Canada",
            "test@example.com",
            null,
            DateTimeOffset.UtcNow.AddDays(7)));

        await Task.Delay(100);
        Assert.False(actionTask.IsCompleted);

        notificationService.Completion.TrySetResult(true);

        var result = await actionTask.WaitAsync(TimeSpan.FromSeconds(1));
        Assert.IsType<OkObjectResult>(result);
        Assert.True(notificationService.WasCalled);
        Assert.Equal("test@example.com", notificationService.Email);
        Assert.Null(notificationService.Phone);
        Assert.True(notificationService.Completion.Task.IsCompleted);
    }

    [Fact]
    public async Task CreateInvitation_ShouldReturnAfterTimeout_WhenNotificationDoesNotFinish()
    {
        var originalDelay = InvitationController.NotificationFailureDelay;
        InvitationController.NotificationFailureDelay = TimeSpan.FromMilliseconds(50);

        try
        {
            using var db = CreateDbContext();
            var notificationService = new RecordingInvitationNotificationService();
            var controller = CreateController(db, notificationService);

            var startedAt = Stopwatch.StartNew();
            var result = await controller.CreateInvitation(new CreateInvitationDto(
                "Test User",
                true,
                "Canada",
                "test@example.com",
                null,
                DateTimeOffset.UtcNow.AddDays(7)));
            startedAt.Stop();

            var objectResult = Assert.IsType<OkObjectResult>(result);
            Assert.IsType<InvitationDto>(objectResult.Value);
            Assert.True(startedAt.Elapsed >= TimeSpan.FromMilliseconds(40));
            Assert.True(notificationService.WasCalled);
        }
        finally
        {
            InvitationController.NotificationFailureDelay = originalDelay;
        }
    }

    private static InvitationController CreateController(EmberDbContext db, IInvitationNotificationService notificationService)
    {
        var controller = new InvitationController(db, notificationService);
        controller.ControllerContext = new ControllerContext
        {
            HttpContext = new DefaultHttpContext
            {
                User = new ClaimsPrincipal(new ClaimsIdentity(new[]
                {
                    new Claim("sub", Guid.NewGuid().ToString()),
                    new Claim(ClaimConstants.AllowToInviteUser, "true")
                }, "Test"))
            }
        };

        return controller;
    }

    private static EmberDbContext CreateDbContext()
    {
        var options = new DbContextOptionsBuilder<EmberDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        var db = new EmberDbContext(options);
        db.Database.EnsureCreated();
        return db;
    }

    private sealed class RecordingInvitationNotificationService : IInvitationNotificationService
    {
        public bool WasCalled { get; private set; }
        public string? Email { get; private set; }
        public string? Phone { get; private set; }
        public TaskCompletionSource<bool> Completion { get; } = new(TaskCreationOptions.RunContinuationsAsynchronously);

        public Task SendInvitationAsync(string realName, string inviteCode, string? email, string? phone)
        {
            WasCalled = true;
            Email = email;
            Phone = phone;
            return Completion.Task;
        }
    }

}