import "@typespec/openapi";
import "@typespec/http";

using Http;
@service(#{ title: "Ember Contracts" })
namespace Ember.WebServer.Models;

@format("uuid")
scalar Guid extends string;

model ListRequestBase
{
    maxCount: int32  = 100;
    continuationToken?: string;
    sorting?: string[];
}

model ListResponseBase
{
    continuationToken?: string;
}

enum ContentTypes
{
    Paragraph,
    Section,
    Article,
    Image,
    Video,
    Audio,
    Link,
    InteractiveElement,
    CodeSnippet,
    ExplorableDataset,
    Comment,
}

enum RelatedContentTypes
{
    Reference,
    SupplementaryMaterial,
    Citation,
    FurtherReading,
    RelatedTopic,
    Alternative,
    Contradicting,
    Exception,
    InstanceOf,
    PartOf,
    Supersedes,
}

model KnowledgeRequestModel extends ListRequestBase
{
    contentIds?: Guid[];
    parentContentIds?: (Guid | null)[];
    contentTypes?: ContentTypes[];
    titleSearchTerm?: string;
    dataSearchTerm?: string;
    createdByUserIds?: Guid[];
    createdAfter?: offsetDateTime;
    unreadByMe?: boolean;
    relationshipFilters?: RelationshipFilters[];
    collections?: Guid[];
}

model RelationshipFilters
{
    relatedContentTypeId: RelatedContentTypes;
    relatedContentId: Guid;
}

model KnowledgeResponseModel extends ListResponseBase
{
    contents?: ContentModel[];
}

model ContentModel
{
    id: Guid;
    parentContentId?: Guid;
    contentTypeId: ContentTypes;
    title?: string;
    data?: string;
    createdByUserId: Guid;
    createdAt: offsetDateTime;
}

@route("/api/v01/Knowledge")
@tag("Ember")
interface KnowledgeService {
  @post @route("GetKnowledgeItems") GetKnowledgeItems(@body request: KnowledgeRequestModel): KnowledgeResponseModel;
}
