using Ember.Domain.Data;

namespace Ember.Domain.EmberEntities;

public class ContentCollection
{
    public Guid Id { get; set; }

    public string? Name { get; set; }
    public string? Description { get; set; }

    public Guid EmberUserId { get; set; }
    public EmberUser? EmberUser { get; set; }

    public DateTimeOffset CreatedAt { get; set; }

    public ICollection<ContentCollectionItem>? CollectionItems { get; set; }
}

public class ContentCollectionItem
{
    public Guid Id { get; set; }

    public Guid CollectionId { get; set; }
    public ContentCollection? Collection { get; set; }

    public Guid? ContentId { get; set; }
    public Content? Content { get; set; }

    public int OrderIndex { get; set; }

    public DateTimeOffset AddedAt { get; set; }
}
