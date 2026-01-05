namespace Ember.WebServer.Areas.Knowledge.Entities;

public class RelatedContent
{
    public Guid Id { get; set; }

    public required Guid ContentId { get; set; }
    public Content? Content { get; set; }

    public required Guid RelatedContentId { get; set; }
    public Content? RelatedContentItem { get; set; }

    public required RelatedContentTypes RelatedContentTypeId { get; set; }
    public RelatedContentType? RelatedContentType { get; set; }
}

public class RelatedContentType
{
    public RelatedContentTypes Id { get; set; }

    public string? Name { get; set; }
}

public enum RelatedContentTypes
{
    Reference = 1,
    SupplementaryMaterial,
    Citation,
    FurtherReading,
    RelatedTopic,
    Alternative,
    Contradicting,
}
