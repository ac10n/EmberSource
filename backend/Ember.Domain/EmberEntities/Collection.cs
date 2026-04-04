using Ember.Domain.Data;

namespace Ember.Domain.EmberEntities;

public class Collection
{
    public Guid Id { get; set; }

    public string? Name { get; set; }
    public string? Description { get; set; }

    public Guid EmberUserId { get; set; }
    public EmberUser? EmberUser { get; set; }

    public DateTimeOffset CreatedAt { get; set; }
}

public class CollectionItem
{
    public Guid Id { get; set; }

    public Guid CollectionId { get; set; }
    public Collection? Collection { get; set; }

    public Guid? ContentId { get; set; }
    public Content? Content { get; set; }

    public int OrderIndex { get; set; }

    public DateTimeOffset AddedAt { get; set; }
}
