using Ember.WebServer.Helpers;
using Newtonsoft.Json;

namespace Ember.Domain.Data;

public class ProcessLog: IScopedLogRecord
{
    public Guid Id { get; set; }

    public Guid? RequestId { get; set; }
    public RequestLog? Request { get; set; }

    public required string CodePath { get; set; }
    public required string? Parameters { get; set; }

    public DateTimeOffset StartTime { get; set; }
    public TimeSpan Duration { get; set; }
    public DateTimeOffset? PurgingTime { get; set; }
}

public record struct ProcessLogArgs(string CodePath, object? Parameters) : ILogRecordCreator<ProcessLog>
{
    public readonly ProcessLog CreateLogRecord(Guid? requestId)
    {
        return new ProcessLog
        {
            RequestId = requestId,
            CodePath = CodePath,
            Parameters = Parameters == null ? null : JsonConvert.SerializeObject(Parameters),
        };
    }
}
