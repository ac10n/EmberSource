using Ember.WebServer.Data;

namespace Ember.WebServer.Areas.Knowledge.Entities;

public class Collection
{
    public Guid Id { get; set; }

    public string? Name { get; set; }
    public string? Description { get; set; }

    public Guid CreatedByUserId { get; set; }
    public EmberUser? CreatedByUser { get; set; }

    public DateTime CreatedAt { get; set; }
}

public class CollectionItem
{
    public Guid Id { get; set; }

    public Guid CollectionId { get; set; }
    public Collection? Collection { get; set; }

    public Guid ContentId { get; set; }
    public Content? Content { get; set; }

    public int OrderIndex { get; set; }

    public DateTime AddedAt { get; set; }
}
