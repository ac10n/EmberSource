namespace Ember.WebServer.Data;

public class InteractionLog: IOwnedData, IPurgableData
{
    public Guid Id { get; set; }

    public required Guid? UserId { get; set; }
    public required EmberUser? User { get; set; }

    public required string RelativeUrl { get; set; }
    public required DateTime Timestamp { get; set; } = DateTime.UtcNow;
    public required string InteractionType { get; set; }
    public string? Details { get; set; }

    public DataOwnershipType Owner { get; set; }
    public DateTime? PurgingTime { get; set; }
}
