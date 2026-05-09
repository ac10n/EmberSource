using Ember.Domain.Data;
using Ember.Domain.EmberEntities;
using Ember.Service;
using Ember.WebServer;
using Ember.Infrastructure;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Hosting;

namespace Ember.WebServer.Tests;

public class TestFixture : WebApplicationFactory<Program>
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        Environment.SetEnvironmentVariable("UseInMemoryDb", "true");
        builder.ConfigureServices(services =>
        {
            // Configure auth options for testing
            services.Configure<Ember.Service.AuthSettings>(options =>
            {
                options.JwtKey = "dummy-jwt-key-for-testing-purposes-only";
                options.JwtIssuer = "test-issuer";
                options.JwtAudience = "test-audience";
                options.AccessTokenMinutes = 15;
                options.RefreshTokenDays = 30;
                options.Smtp.Host = "localhost";
                options.Smtp.Username = "test";
                options.Smtp.Password = "test";
                options.Facebook.AppId = "test";
                options.Facebook.AppSecret = "test";
                options.Google.ClientId = "test";
                options.Google.ClientSecret = "test";
            });

            // Register dummy email sender for tests
            services.AddScoped<Ember.WebServer.Areas.People.Services.IEmailSender, DummyEmailSender>();

            // Ensure the context is created
            var sp = services.BuildServiceProvider();
            using var scope = sp.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<EmberDbContext>();
            db.Database.EnsureCreated();
        });

        builder.ConfigureAppConfiguration((context, config) =>
        {
            // Add dummy config for auth and test db
            config.AddInMemoryCollection(new Dictionary<string, string?>
            {
                ["UseInMemoryDb"] = "true",
                ["AuthSettings:JwtKey"] = "dummy-jwt-key-for-testing-purposes-only",
                ["AuthSettings:JwtIssuer"] = "test-issuer",
                ["AuthSettings:JwtAudience"] = "test-audience",
                ["AuthSettings:AccessTokenMinutes"] = "15",
                ["AuthSettings:RefreshTokenDays"] = "30",
                ["Secrets:FounderPassword"] = "ValidPassword123!",
                ["AuthSettings:Smtp:Host"] = "localhost",
                ["AuthSettings:Smtp:Username"] = "test",
                ["AuthSettings:Smtp:Password"] = "test",
                ["AuthSettings:Facebook:AppId"] = "test",
                ["AuthSettings:Facebook:AppSecret"] = "test",
                ["AuthSettings:Google:ClientId"] = "test",
                ["AuthSettings:Google:ClientSecret"] = "test"
            });
        });
    }

    public void SeedRequiredData()
    {
        using var scope = Services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<EmberDbContext>();

        // Seed ContentTypes, etc.
        if (!db.ContentTypes.Any())
        {
            db.ContentTypes.Add(new ContentType { Id = ContentTypes.Paragraph, Name = "Paragraph" });
        }
        if (!db.ContentFormats.Any())
        {
            db.ContentFormats.Add(new ContentFormat { Id = ContentFormatEnum.Markdown, Name = "Markdown" });
        }
        if (!db.ContentVisibilities.Any())
        {
            db.ContentVisibilities.Add(new ContentVisibility { Id = ContentVisibilityEnum.Public, Name = "Public" });
        }
        db.SaveChanges();
    }

    public async Task<Invitation> CreateTestInvitation(string realName = "Test User", string jurisdiction = "Test", bool isLegalAge = true, string? email = null, string? phone = null)
    {
        using var scope = Services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<EmberDbContext>();

        var invitation = new Invitation
        {
            Id = Guid.NewGuid(),
            InvitedByUserId = Guid.NewGuid(), // dummy
            RealName = realName,
            IsInLegalAge = isLegalAge,
            Jurisdiction = jurisdiction,
            Email = email,
            Phone = phone,
            InviteCode = Guid.NewGuid().ToString(),
            CreatedAt = DateTimeOffset.UtcNow,
            ExpiresAt = DateTimeOffset.UtcNow.AddDays(7)
        };

        db.Invitations.Add(invitation);
        await db.SaveChangesAsync();
        return invitation;
    }

    public async Task<Content> CreateTestContent(Guid userId)
    {
        using var scope = Services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<EmberDbContext>();

        var content = new Content
        {
            Id = Guid.NewGuid(),
            Identifier = Guid.NewGuid(),
            Version = 1,
            Title = "Test Content",
            Data = "Test data",
            ContentTypeId = ContentTypes.Paragraph,
            ContentFormatId = ContentFormatEnum.Markdown,
            FormatVersion = 1,
            ContentVisibilityId = ContentVisibilityEnum.Public,
            EmberUserId = userId,
            CreatedAt = DateTimeOffset.UtcNow,
            IsActive = true
        };

        db.Contents.Add(content);
        await db.SaveChangesAsync();
        return content;
    }
}