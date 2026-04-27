using Ember.Domain.EmberEntities;
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
    public DateTimeOffset? CreatedAfter { get; set; }
    public bool? UnreadByMe { get; set; }
    public IEnumerable<RelationshipFilters>? RelationshipFilters { get; set; }
    public IEnumerable<Guid>? Collections { get; set; }
}

public class RelationshipFilters
{
    public required RelatedContentTypeEnum RelatedContentTypeId { get; set; }
    public required Guid RelatedContentId { get; set; }
}

public class KnowledgeResponseModel: ListResponseBase
{
    public IEnumerable<ContentModel>? Contents { get; set; }
}

public class ContentModel
{
    public Guid Id { get; set; }
    public Guid Identifier { get; set; }

    public Guid? ParentContentId { get; set; }

    public ContentTypes ContentTypeId { get; set; }
    public ContentFormatEnum ContentFormatId { get; set; }
    public ContentVisibilityEnum ContentVisibilityId { get; set; }
    public string? VisibilityCriteria { get; set; }

    public string? Title { get; set; }
    public string? Data { get; set; }

    public Guid EmberUserId { get; set; }

    public DateTimeOffset CreatedAt { get; set; }
}

// New models for content modification
public class ContentCreateModel
{
    public Guid? ParentContentId { get; set; }
    public required ContentTypes ContentTypeId { get; set; }
    public required ContentFormatEnum ContentFormatId { get; set; }
    public int FormatVersion { get; set; } = 1;
    public string? Title { get; set; }
    public required string Data { get; set; }
    public required ContentVisibilityEnum ContentVisibilityId { get; set; }
    public string? VisibilityCriteria { get; set; }
    public IEnumerable<TagModel>? Tags { get; set; }
    public IEnumerable<Guid>? CollectionIds { get; set; }
}

public class ContentUpdateModel
{
    public Guid? ParentContentId { get; set; }
    public ContentTypes ContentTypeId { get; set; }
    public ContentFormatEnum ContentFormatId { get; set; }
    public int FormatVersion { get; set; } = 1;
    public string? Title { get; set; }
    public required string Data { get; set; }
    public ContentVisibilityEnum ContentVisibilityId { get; set; }
    public string? VisibilityCriteria { get; set; }
    public IEnumerable<TagModel>? Tags { get; set; }
    public IEnumerable<Guid>? CollectionIds { get; set; }
}

// Tag models
public class TagModel
{
    public Guid Id { get; set; }
    public string? Name { get; set; }
    public bool IsPrivate { get; set; }
    public bool HasConfidenceRate { get; set; }
    public byte? ConfidenceRate { get; set; }
}

public class TagCreateModel
{
    public required string Name { get; set; }
    public bool IsPrivate { get; set; }
    public bool HasConfidenceRate { get; set; }
}

public class TagUpdateModel
{
    public string? Name { get; set; }
    public bool IsPrivate { get; set; }
    public bool HasConfidenceRate { get; set; }
}

// Collection models
public class CollectionModel
{
    public Guid Id { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
    public Guid EmberUserId { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
    public IEnumerable<CollectionItemModel>? Items { get; set; }
}

public class CollectionCreateModel
{
    public required string Name { get; set; }
    public string? Description { get; set; }
}

public class CollectionUpdateModel
{
    public string? Name { get; set; }
    public string? Description { get; set; }
}

public class CollectionItemModel
{
    public Guid Id { get; set; }
    public Guid? ContentId { get; set; }
    public Guid CollectionId { get; set; }
    public int OrderIndex { get; set; }
    public DateTimeOffset AddedAt { get; set; }
    public ContentModel? Content { get; set; }
}

public class CollectionItemCreateModel
{
    public Guid? ContentId { get; set; }
    public int OrderIndex { get; set; }
}

// Testimonial models
public class TestimonialModel
{
    public Guid Id { get; set; }
    public Guid ByEmberUserId { get; set; }
    public Guid ForEmberUserId { get; set; }
    public Guid BadgeDefinitionId { get; set; }
    public bool? ApprovesBooleanBadge { get; set; }
    public decimal? NumericBadgeValue { get; set; }
    public string? Message { get; set; }
    public DateTimeOffset FromTime { get; set; }
    public DateTimeOffset? ToTime { get; set; }
}

public class TestimonialCreateModel
{
    public required Guid ForEmberUserId { get; set; }
    public required Guid BadgeDefinitionId { get; set; }
    public bool? ApprovesBooleanBadge { get; set; }
    public decimal? NumericBadgeValue { get; set; }
    public string? Message { get; set; }
    public required DateTimeOffset FromTime { get; set; }
    public DateTimeOffset? ToTime { get; set; }
}

public class TestimonialUpdateModel
{
    public Guid BadgeDefinitionId { get; set; }
    public bool? ApprovesBooleanBadge { get; set; }
    public decimal? NumericBadgeValue { get; set; }
    public string? Message { get; set; }
    public DateTimeOffset FromTime { get; set; }
    public DateTimeOffset? ToTime { get; set; }
}

