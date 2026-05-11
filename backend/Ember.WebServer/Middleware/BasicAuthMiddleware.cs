using System.Text;

namespace Ember.WebServer.Middleware;

public sealed class BasicAuthMiddleware(RequestDelegate next, IConfiguration configuration)
{
    private readonly string _expectedUser = configuration["Basic:user"]
        ?? throw new InvalidOperationException("Missing configuration value 'Basic:user'.");

    private readonly string _expectedPass = configuration["Basic:Pass"]
        ?? throw new InvalidOperationException("Missing configuration value 'Basic:Pass'.");

    public async Task InvokeAsync(HttpContext context)
    {
        if (IsAuthorized(context.Request.Headers.Authorization))
        {
            await next(context);
            return;
        }

        context.Response.StatusCode = StatusCodes.Status401Unauthorized;
        context.Response.Headers.WWWAuthenticate = "Basic realm=\"Scalar\"";
        await context.Response.WriteAsync("Authentication required.");
    }

    private bool IsAuthorized(string? authorizationHeader)
    {
        if (string.IsNullOrWhiteSpace(authorizationHeader) ||
            !authorizationHeader.StartsWith("Basic ", StringComparison.OrdinalIgnoreCase))
        {
            return false;
        }

        var encodedCredentials = authorizationHeader[6..].Trim();
        if (string.IsNullOrEmpty(encodedCredentials))
        {
            return false;
        }

        string decodedCredentials;
        try
        {
            decodedCredentials = Encoding.UTF8.GetString(Convert.FromBase64String(encodedCredentials));
        }
        catch (FormatException)
        {
            return false;
        }

        var separatorIndex = decodedCredentials.IndexOf(':');
        if (separatorIndex <= 0)
        {
            return false;
        }

        var user = decodedCredentials[..separatorIndex];
        var pass = decodedCredentials[(separatorIndex + 1)..];

        return string.Equals(user, _expectedUser, StringComparison.Ordinal) &&
            string.Equals(pass, _expectedPass, StringComparison.Ordinal);
    }
}