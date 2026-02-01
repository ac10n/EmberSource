namespace Ember.WebServer.Data;

public class ActionLog: IOwnedData, IPurgableData
{
    public Guid Id { get; set; }

    public Guid? RequestId { get; set; }
    public RequestLog? Request { get; set; }

    public Guid? UserId { get; set; }
    public EmberUser? User { get; set; }

    public required DateTimeOffset Timestamp { get; set; } = DateTime.UtcNow;
    public required string ActionType { get; set; }
    public string? Details { get; set; }
    public string? OldValues { get; set; }
    public string? NewValues { get; set; }

    public DateTimeOffset? PurgingTime { get; set; }
    public DataOwnershipType Owner { get; set; } = DataOwnershipType.System;
}
