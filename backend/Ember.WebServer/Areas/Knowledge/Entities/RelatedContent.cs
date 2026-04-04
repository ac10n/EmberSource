using System.ComponentModel.DataAnnotations.Schema;

namespace Ember.WebServer.Areas.Knowledge.Entities;

/// <summary>
/// Users can link content to other contents.
/// </summary>
public class RelatedContent
{
    public Guid Id { get; set; }

    public required Guid ContentId { get; set; }
    [ForeignKey(nameof(ContentId))]
    public Content? Content { get; set; }

    public required Guid RelatedContentId { get; set; }
    [ForeignKey(nameof(RelatedContentId))]
    public Content? RelatedContentItem { get; set; }

    public required RelatedContentTypeEnum RelatedContentTypeId { get; set; }
    public RelatedContentType? RelatedContentType { get; set; }
}

public class RelatedContentType
{
    public RelatedContentTypeEnum Id { get; set; }

    public string? Name { get; set; }
}

public enum RelatedContentTypeEnum
{
    Reference = 1,
    SupplementaryMaterial,
    Citation,
    FurtherReading,
    RelatedTopic,
    Alternative,
    Contradicting,
}
