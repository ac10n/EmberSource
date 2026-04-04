using System.Text;
using Ember.Service;
using Ember.WebServer.Areas.People.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

namespace Ember.WebServer.Areas.People.Config;

public static class AuthExtensions
{
    public static void ConfigureAuth(this WebApplicationBuilder builder)
    {
        builder.Services.Configure<JwtOptions>(builder.Configuration.GetSection("Jwt"));

        var jwt = builder.Configuration.GetSection("Jwt").Get<JwtOptions>()!;
        var keyBytes = Encoding.UTF8.GetBytes(jwt.SigningKey);

        builder.Services
            .AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidIssuer = jwt.Issuer,

                    ValidateAudience = true,
                    ValidAudience = jwt.Audience,

                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(keyBytes),

                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.FromSeconds(30),

                    NameClaimType = "sub",
                    RoleClaimType = "role"
                };

                // options.Events = new JwtBearerEvents
                // {
                //     OnAuthenticationFailed = ctx =>
                //     {
                //         ctx.NoResult();
                //         ctx.Response.Headers["auth-error"] = ctx.Exception.GetType().Name;
                //         ctx.Response.Headers["auth-error-desc"] = ctx.Exception.Message;
                //         return Task.CompletedTask;
                //     }
                // };
            });

        builder.Services.AddAuthorization();

        builder.Services.AddScoped<TokenService>();
    }
}
