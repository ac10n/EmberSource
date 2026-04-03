namespace Ember.WebServer.Data;

public class ResponseLog
{
    public Guid Id { get; set; }

    public required Guid RequestLogId { get; set; }
    public required RequestLog? RequestLog { get; set; }

    public required int ResponseStatusCode { get; set; }
    public TimeSpan ProcessingTime { get; set; }
    public long TotalResponseSize { get; set; }
    public DateTimeOffset? PurgingTime { get; set; }
}
