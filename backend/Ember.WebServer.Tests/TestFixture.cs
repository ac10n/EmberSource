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
            // Configure JWT options for testing
            services.Configure<Ember.Service.JwtOptions>(options =>
            {
                options.SigningKey = "dummy-jwt-key-for-testing-purposes-only";
                options.Issuer = "test-issuer";
                options.Audience = "test-audience";
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
                ["Jwt:SigningKey"] = "dummy-jwt-key-for-testing-purposes-only",
                ["Jwt:Issuer"] = "test-issuer",
                ["Jwt:Audience"] = "test-audience",
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