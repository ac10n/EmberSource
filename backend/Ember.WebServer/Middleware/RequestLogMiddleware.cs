using Ember.WebServer.Data;

public interface IRequestLogContext
{
    Guid? RequestLogId { get; }
    RequestLog? Entity { get; }        // optional, you can keep only the Id
    void Set(RequestLog entity);
}

public sealed class RequestLoggingMiddleware(RequestDelegate next)
{
    public async Task InvokeAsync(
        HttpContext http,
        EmberDbContext db,
        IRequestLogContext ctx)
    {
        Guid? userId = GetUserId(http);

        var log = new RequestLog
        {
            UserId = userId,
            HttpMethod = http.Request.Method,
            RelativeUrl = http.Request.Path.Value ?? "",
            Timestamp = DateTime.UtcNow,
            BaseUri = $"{http.Request.Scheme}://{http.Request.Host}",
            TotalRequestSize = http.Request?.ContentLength ?? 0,
            QueryString = http.Request?.QueryString.Value,
            Payload = null // Todo: read body  
        };

        db.Add(log);
        await db.SaveChangesAsync();

        ctx.Set(log);

        try
        {
            await next(http);
        }
        finally
        {
        }
    }

    private Guid? GetUserId(HttpContext http)
    {
        if (http.User?.Identity?.IsAuthenticated != true)
        {
            return null;
        }
        var userIdStr = http.User.FindFirst("sub")?.Value ?? http.User.Identity?.Name;
        if (Guid.TryParse(userIdStr, out var userId))
        {
            return userId;
        }
        return null;
    }
}

public sealed class RequestLogContext : IRequestLogContext
{
    public Guid? RequestLogId => Entity?.Id;
    public Guid? UserId => Entity?.UserId;
    public RequestLog? Entity { get; private set; }
    public void Set(RequestLog entity) => Entity = entity;
}
