using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Ember.WebServer.Data;
using Microsoft.AspNetCore.HttpOverrides;
using Ember.WebServer;
using Ember.WebServer.Areas.Knowledge.Services;
using Ember.WebServer.Areas.Identity.Config;

if (args.Length > 0 && args[0] == "generate-ts-models")
{
    new TypeScriptModelGenerator().GenerateTypeScriptModels();
    return;
}

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddIdentity<EmberUser, EmberRole>(options =>
        {
            options.SignIn.RequireConfirmedAccount = false;
            options.Password.RequireNonAlphanumeric = false;
        })
    .AddEntityFrameworkStores<EmberDbContext>();

builder.ConfigureAuth();

builder.Services.AddScoped<IKnowledgeService, KnowledgeService>();

builder.Services.AddRazorPages();

ServiceExtensions.AddEmberExtensions(builder);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    using var scope = app.Services.CreateScope();
    await IdentitySeeder.SeedAsync(scope.ServiceProvider);
}

app.UseForwardedHeaders(new ForwardedHeadersOptions
{
    ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
});

if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Error");
}

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.UseMiddleware<RequestLoggingMiddleware>();

app.MapStaticAssets();
app.MapRazorPages()
   .WithStaticAssets();

app.MapControllers();

app.Run();
