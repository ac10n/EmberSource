namespace Ember.Domain.Data;


public class UserBadgeValue
{
    public Guid Id { get; set; }

    public required Guid EmberUserId { get; set; }
    public required EmberUser EmberUser { get; set; }

    public required Guid BadgeDefinitionId { get; set; }
    public required BadgeDefinition BadgeDefinition { get; set; }

    public decimal? Value { get; set; }

    public DateTimeOffset FromTime { get; set; }
    public bool IsActive { get; set; }
    public DateTimeOffset? ToTime { get; set; }
}
