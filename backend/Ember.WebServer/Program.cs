using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Ember.WebServer.Data;
using Microsoft.AspNetCore.HttpOverrides;
using Ember.WebServer;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddDefaultIdentity<EmberUser>(options => options.SignIn.RequireConfirmedAccount = false)
    .AddEntityFrameworkStores<EmberDbContext>();
builder.Services.AddRazorPages();

ServiceExtensions.AddEmberExtensions(builder);

var app = builder.Build();

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

app.UseAuthorization();

app.UseMiddleware<RequestLoggingMiddleware>();

app.MapStaticAssets();
app.MapRazorPages()
   .WithStaticAssets();

app.Run();
