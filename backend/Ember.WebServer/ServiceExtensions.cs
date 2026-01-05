using Ember.WebServer.Data;
using Ember.WebServer.Helpers;

namespace Ember.WebServer;

public static class ServiceExtensions
{
    public static WebApplicationBuilder AddEmberExtensions(this WebApplicationBuilder builder)
    {
        var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
        builder.Services.AddDbContext<EmberDbContext>(options => options.UseNpgsql(connectionString));

        builder.Services.AddScoped<IRequestLogContext, RequestLogContext>();
        builder.Services.AddScoped<ILogHelper, LogHelper>();

        return builder;
    }
}
