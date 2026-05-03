using System.Text;
using Ember.Service;
using Ember.WebServer.Areas.People.Services;
using Ember.WebServer.Helpers;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using ClaimConstants = Ember.Service.ClaimConstants;

namespace Ember.WebServer.Areas.People.Config;

public static class AuthExtensions
{
    public static void ConfigureAuth(this WebApplicationBuilder builder)
    {
        builder.Services.Configure<AuthSettings>(builder.Configuration.GetSection("AuthSettings"));

        var authSettings = builder.Configuration.GetSection("AuthSettings").Get<AuthSettings>();
        if (authSettings == null || string.IsNullOrEmpty(authSettings.JwtKey))
        {
            authSettings = new AuthSettings
            {
                JwtKey = "default-signing-key-for-development",
                JwtIssuer = "default-issuer",
                JwtAudience = "default-audience"
            };
        }
        var keyBytes = Encoding.UTF8.GetBytes(authSettings.JwtKey);

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
                    ValidIssuer = authSettings.JwtIssuer,

                    ValidateAudience = true,
                    ValidAudience = authSettings.JwtAudience,

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

        builder.Services.AddAuthorization(options =>
        {
            options.AddPolicy(PolicyConstants.AllowToInviteUser, policy =>
                policy.RequireClaim(ClaimConstants.AllowToInviteUser, "true"));

            options.AddPolicy(PolicyConstants.AllowToRegisterUser, policy =>
                policy.RequireClaim(ClaimConstants.AllowToRegisterUser, "true"));
        });

        builder.Services.AddScoped<TokenService>();
    }
}
