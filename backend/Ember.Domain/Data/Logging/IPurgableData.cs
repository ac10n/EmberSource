namespace Ember.Domain.Data;

public interface IPurgableData
{
    // If PurgingTime is set, the data is expected to be purged after this time
    public DateTimeOffset? PurgingTime { get; set; }
}
