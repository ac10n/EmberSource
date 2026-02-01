namespace Ember.WebServer.Data;

public class Testimonial
{
    public Guid Id { get; set; }

    public required Guid ByUserId { get; set; }
    public required EmberUser ByUser { get; set; }

    public required Guid ForUserId { get; set; }
    public required EmberUser ForUser { get; set; }

    public required Guid BadgeDefinitionId { get; set; }
    public required BadgeDefinition BadgeDefinition { get; set; }

    public bool? ApprovesBooleanBadge { get; set; }
    public decimal? NumericBadgeValue { get; set; }

    public string? Message { get; set; }

    public DateTimeOffset FromTime { get; set; }
    public DateTimeOffset? ToTime { get; set; }
}
