namespace Ember.Domain.Data;
/// <summary>
/// User oponions about other users, they can be used to give feedback, recognition, or endorsements within the platform.
/// </summary>
public class Testimonial
{
    public Guid Id { get; set; }

    public required Guid ByEmberUserId { get; set; }
    public required EmberUser ByEmberUser { get; set; }

    public required Guid ForEmberUserId { get; set; }
    public required EmberUser ForEmberUser { get; set; }
    public required Guid BadgeDefinitionId { get; set; }
    public required BadgeDefinition BadgeDefinition { get; set; }

    public bool? ApprovesBooleanBadge { get; set; }
    public decimal? NumericBadgeValue { get; set; }

    public string? Message { get; set; }

    public DateTimeOffset FromTime { get; set; }
    public DateTimeOffset? ToTime { get; set; }
}
