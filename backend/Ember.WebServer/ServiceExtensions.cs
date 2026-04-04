using Ember.Domain.Data;
using Ember.Infrastructure;
using Ember.Service;
using Ember.WebServer.Areas.Knowledge.Services;
using Ember.WebServer.Helpers;

namespace Ember.WebServer;

public static class ServiceExtensions
{
    public static WebApplicationBuilder AddEmberExtensions(this WebApplicationBuilder builder)
    {
        var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
        builder.Services.AddDbContext<EmberDbContext>(options => options.UseNpgsql(connectionString));

        builder.Services.AddScoped<IKnowledgeService, KnowledgeService>();
        builder.Services.AddScoped<IRequestLogContext, RequestLogContext>();
        builder.Services.AddScoped<ILogHelper, LogHelper>();
        builder.Services.AddScoped<IEmberDbContext>(sp => sp.GetRequiredService<EmberDbContext>());

        return builder;
    }
}
