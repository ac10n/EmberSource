using Ember.WebServer.Areas.Knowledge.Models;
using Ember.WebServer.Data;
using Ember.WebServer.Helpers;

namespace Ember.WebServer.Areas.Knowledge.Services;

public interface IKnowledgeService
{
    Task<KnowledgeResponseModel> GetKnowledgeItems(KnowledgeRequestModel request);
}

public class KnowledgeService(IServiceProvider serviceProvider): IKnowledgeService
{
    private Lazy<EmberDbContext> DbContext => serviceProvider.Lazy<EmberDbContext>();
    private Lazy<ILogHelper> LogHelper => serviceProvider.Lazy<ILogHelper>();
    private Lazy<RequestLogContext> RequestLogContext => serviceProvider.Lazy<RequestLogContext>();

    public async Task<KnowledgeResponseModel> GetKnowledgeItems(KnowledgeRequestModel request)
    {
        await using var processLog = LogHelper.Value.BeginLogScope<ProcessLog, ProcessLogArgs>(new ProcessLogArgs($"{nameof(KnowledgeService)}.{nameof(GetKnowledgeItems)}", request));
        var query = DbContext.Value.Contents.AsQueryable();

        if (request.ContentIds is not null && request.ContentIds.Any())
        {
            query = query.Where(c => request.ContentIds.Contains(c.Id));
        }

        if (request.ParentContentIds is not null && request.ParentContentIds.Any())
        {
            query = query.Where(c => request.ParentContentIds.Contains(c.ParentContentId));
        }

        if (request.ContentTypes is not null && request.ContentTypes.Any())
        {
            query = query.Where(c => request.ContentTypes.Contains(c.ContentTypeId));
        }

        if (!string.IsNullOrWhiteSpace(request.TitleSearchTerm))
        {
            query = query.Where(c => EF.Functions.ILike(c.Title!, $"%{request.TitleSearchTerm}%"));
        }

        if (!string.IsNullOrWhiteSpace(request.DataSearchTerm))
        {
            query = query.Where(c => EF.Functions.ILike(c.Data!, $"%{request.DataSearchTerm}%"));
        }

        if (request.CreatedByUserIds is not null && request.CreatedByUserIds.Any())
        {
            query = query.Where(c => request.CreatedByUserIds.Contains(c.CreatedByUserId));
        }

        if (request.CreatedAfter.HasValue)
        {
            query = query.Where(c => c.CreatedAt >= request.CreatedAfter.Value);
        }

        if (request.UnreadByMe.HasValue && request.UnreadByMe.Value)
        {
            query = query.Where(c => !DbContext.Value.ContentInteractions
                .Any(ci => ci.ContentId == c.Id && ci.UserId == RequestLogContext.Value.UserId && ci.IsRead));
        }

        if (request.RelationshipFilters is not null && request.RelationshipFilters.Any())
        {
            query = query.Where(c => request.RelationshipFilters.Any(rf =>
                DbContext.Value.RelatedContents.Any(cr =>
                    cr.ContentId == c.Id &&
                    cr.RelatedContentTypeId == rf.RelatedContentTypeId &&
                    cr.RelatedContentId == rf.RelatedContentId)));
        }

        if (request.Collections is not null && request.Collections.Any())
        {
            query = query.Where(c => DbContext.Value.CollectionItems.Any(cc => cc.ContentId == c.Id && request.Collections.Contains(cc.CollectionId)));
        }

        var projectedQuery = query.Select(c => new ContentModel
        {
            Id = c.Id,
            ParentContentId = c.ParentContentId,
            ContentTypeId = c.ContentTypeId,
            Title = c.Title,
            Data = c.Data,
            CreatedByUserId = c.CreatedByUserId,
            CreatedAt = c.CreatedAt
        });

        var items = await projectedQuery.ToListAsync();

        return new KnowledgeResponseModel
        {
            Contents = items,
        };
    }
}
