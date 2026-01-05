using Ember.WebServer.Areas.Knowledge.Entities;
using Ember.WebServer.Models;

namespace Ember.WebServer.Areas.Knowledge.Models;

public class KnowledgeRequestModel: ListRequestBase
{
    public IEnumerable<Guid>? ContentIds { get; set; }
    public IEnumerable<Guid?>? ParentContentIds { get; set; }
    public IEnumerable<ContentTypes>? ContentTypes { get; set; }
    public string? TitleSearchTerm { get; set; }
    public string? DataSearchTerm { get; set; }
    public IEnumerable<Guid>? CreatedByUserIds { get; set; }
    public DateTime? CreatedAfter { get; set; }
    public bool? UnreadByMe { get; set; }
    public IEnumerable<RelationshipFilters>? RelationshipFilters { get; set; }
    public IEnumerable<Guid>? Collections { get; set; }
}

public class RelationshipFilters
{
    public required RelatedContentTypes RelatedContentTypeId { get; set; }
    public required Guid RelatedContentId { get; set; }
}

public class KnowledgeResponseModel: ListResponseBase
{
    public IEnumerable<ContentModel>? Contents { get; set; }
}

public class ContentModel
{
    public Guid Id { get; set; }

    public Guid? ParentContentId { get; set; }

    public ContentTypes ContentTypeId { get; set; }

    public string? Title { get; set; }
    public string? Data { get; set; }

    public Guid CreatedByUserId { get; set; }

    public DateTime CreatedAt { get; set; }
}

