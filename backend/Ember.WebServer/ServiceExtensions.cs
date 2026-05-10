using Ember.Domain.Data;
using Ember.Infrastructure;
using Ember.Service;
using Ember.WebServer.Areas.Knowledge.Services;
using PeopleServices = Ember.WebServer.Areas.People.Services;
using Ember.WebServer.Helpers;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace Ember.WebServer;

public static class ServiceExtensions
{
    public static WebApplicationBuilder AddEmberExtensions(this WebApplicationBuilder builder)
    {
        var useInMemory = Environment.GetEnvironmentVariable("UseInMemoryDb") == "true";
        if (useInMemory)
        {
            builder.Services.AddDbContext<EmberDbContext>(options => options.UseInMemoryDatabase("TestDb"));
        }
        else
        {
            var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
            builder.Services.AddDbContext<EmberDbContext>(options => options.UseNpgsql(connectionString));
        }

        builder.Services.AddScoped<IKnowledgeService, KnowledgeService>();
        builder.Services.AddScoped<IRequestLogContext, RequestLogContext>();
        builder.Services.AddScoped<ILogHelper, LogHelper>();
        builder.Services.AddSingleton<AuthSettings>(sp => sp.GetRequiredService<IOptions<AuthSettings>>().Value);
        builder.Services.AddScoped<PeopleServices.IEmailSender, PeopleServices.EmailSender>();
        builder.Services.AddScoped<PeopleServices.ISmsSender, PeopleServices.TwilioSmsSender>();
        builder.Services.AddScoped<PeopleServices.IInvitationNotificationService, PeopleServices.InvitationNotificationService>();
        builder.Services.AddScoped<IEmberDbContext>(sp => sp.GetRequiredService<EmberDbContext>());

        return builder;
    }
}
