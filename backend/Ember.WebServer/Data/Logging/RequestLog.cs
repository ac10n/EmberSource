namespace Ember.WebServer.Data;

public class RequestLog
{
    public Guid Id { get; set; }

    public Guid? UserId { get; set; }
    public EmberUser? User { get; set; }

    public required string HttpMethod { get; set; }
    public required string RelativeUrl { get; set; }
    public string? QueryString { get; set; }
    public required DateTime Timestamp { get; set; }
    public long TotalRequestSize { get; set; }
    public string? Payload { get; set; }
    public required string BaseUri { get; set; }
}
