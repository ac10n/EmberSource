using Ember.WebServer.Areas.Knowledge.Models;
using Ember.Domain.Data;
using Ember.WebServer.Helpers;
using Ember.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Ember.Domain.EmberEntities;
using System.Linq;

namespace Ember.WebServer.Areas.Knowledge.Services;

public interface IKnowledgeService
{
    Task<KnowledgeResponseModel> GetKnowledgeItems(KnowledgeRequestModel request);
    Task<ContentModel> AddModifyContent(ContentCreateModel createModel);
    Task<ContentModel> AddModifyContent(Guid contentId, ContentUpdateModel updateModel);
    Task DeactivateContent(Guid contentId);
    Task<TagModel> CreateTag(TagCreateModel createModel);
    Task<TagModel> UpdateTag(Guid tagId, TagUpdateModel updateModel);
    Task<IEnumerable<TagModel>> GetTags();
    Task<TagModel?> GetTag(Guid tagId);
    Task DeleteTag(Guid tagId);
    Task<CollectionModel> CreateCollection(CollectionCreateModel createModel);
    Task<CollectionModel> UpdateCollection(Guid collectionId, CollectionUpdateModel updateModel);
    Task<IEnumerable<CollectionModel>> GetCollections();
    Task<CollectionModel?> GetCollection(Guid collectionId);
    Task AddContentToCollection(Guid collectionId, CollectionItemCreateModel itemModel);
    Task RemoveContentFromCollection(Guid collectionId, Guid contentId);
    Task DeleteCollection(Guid collectionId);
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
            query = query.Where(c => request.CreatedByUserIds.Contains(c.EmberUserId));
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
            Identifier = c.Identifier,
            ParentContentId = c.ParentContentId,
            ContentTypeId = c.ContentTypeId,
            ContentFormatId = c.ContentFormatId,
            ContentVisibilityId = c.ContentVisibilityId,
            VisibilityCriteria = c.VisibilityCriteria,
            Title = c.Title,
            Data = c.Data,
            EmberUserId = c.EmberUserId,
            CreatedAt = c.CreatedAt
        });

        var items = await projectedQuery.ToListAsync();

        return new KnowledgeResponseModel
        {
            Contents = items,
        };
    }

    public async Task<ContentModel> AddModifyContent(ContentCreateModel createModel)
    {
        await using var processLog = LogHelper.Value.BeginLogScope<ProcessLog, ProcessLogArgs>(new ProcessLogArgs($"{nameof(KnowledgeService)}.{nameof(AddModifyContent)}", createModel));

        var userId = RequestLogContext.Value.UserId ?? throw new InvalidOperationException("User ID not found in request context");

        var content = new Content
        {
            Id = Guid.NewGuid(),
            Identifier = Guid.NewGuid(),
            Version = 1,
            ParentContentId = createModel.ParentContentId,
            ContentTypeId = createModel.ContentTypeId,
            ContentFormatId = createModel.ContentFormatId,
            FormatVersion = createModel.FormatVersion,
            Title = createModel.Title,
            Data = createModel.Data,
            EmberUserId = userId,
            CreatedAt = DateTimeOffset.UtcNow,
            IsActive = true,
            ContentVisibilityId = createModel.ContentVisibilityId,
            VisibilityCriteria = createModel.VisibilityCriteria
        };

        DbContext.Value.Contents.Add(content);

        // Add tags if provided
        if (createModel.Tags is not null && createModel.Tags.Any())
        {
            foreach (var tagModel in createModel.Tags)
            {
                var tag = await GetOrCreateTag(tagModel);
                var tagItem = new TagItem
                {
                    Id = Guid.NewGuid(),
                    ContentId = content.Id,
                    TagId = tag.Id,
                    EmberUserId = userId,
                    ConfidenceRate = tagModel.ConfidenceRate,
                    IsPrivate = tag.IsPrivate
                };
                DbContext.Value.ContentTags.Add(tagItem);
            }
        }

        // Add to collections if provided
        if (createModel.CollectionIds is not null && createModel.CollectionIds.Any())
        {
            foreach (var collectionId in createModel.CollectionIds)
            {
                var collectionItem = new ContentCollectionItem
                {
                    Id = Guid.NewGuid(),
                    CollectionId = collectionId,
                    ContentId = content.Id,
                    OrderIndex = 0, // Default order
                    AddedAt = DateTimeOffset.UtcNow
                };
                DbContext.Value.CollectionItems.Add(collectionItem);
            }
        }

        await DbContext.Value.SaveChangesAsync();

        return new ContentModel
        {
            Id = content.Id,
            Identifier = content.Identifier,
            ParentContentId = content.ParentContentId,
            ContentTypeId = content.ContentTypeId,
            ContentFormatId = content.ContentFormatId,
            ContentVisibilityId = content.ContentVisibilityId,
            VisibilityCriteria = content.VisibilityCriteria,
            Title = content.Title,
            Data = content.Data,
            EmberUserId = content.EmberUserId,
            CreatedAt = content.CreatedAt
        };
    }

    public async Task<ContentModel> AddModifyContent(Guid contentId, ContentUpdateModel updateModel)
    {
        await using var processLog = LogHelper.Value.BeginLogScope<ProcessLog, ProcessLogArgs>(new ProcessLogArgs($"{nameof(KnowledgeService)}.{nameof(AddModifyContent)}", new { contentId, updateModel }));

        var existing = await DbContext.Value.Contents.FirstOrDefaultAsync(x => x.Identifier == contentId && x.IsActive);
        if (existing == null)
        {
            throw new KeyNotFoundException($"Content with ID {contentId} not found or inactive.");
        }

        var userId = RequestLogContext.Value.UserId ?? throw new InvalidOperationException("User ID not found in request context");
        if (existing.EmberUserId != userId)
        {
            throw new UnauthorizedAccessException("Cannot modify content owned by another user.");
        }

        // Soft delete current version
        existing.IsActive = false;
        existing.RemovedTime = DateTimeOffset.UtcNow;

        // Create new version
        var newVersion = new Content
        {
            Id = Guid.NewGuid(),
            Identifier = existing.Identifier,
            Version = existing.Version + 1,
            ParentContentId = updateModel.ParentContentId ?? existing.ParentContentId,
            ContentTypeId = updateModel.ContentTypeId,
            ContentFormatId = updateModel.ContentFormatId,
            FormatVersion = updateModel.FormatVersion,
            Title = updateModel.Title ?? existing.Title,
            Data = updateModel.Data,
            EmberUserId = userId,
            CreatedAt = DateTimeOffset.UtcNow,
            IsActive = true,
            ContentVisibilityId = updateModel.ContentVisibilityId,
            VisibilityCriteria = updateModel.VisibilityCriteria ?? existing.VisibilityCriteria
        };

        DbContext.Value.Contents.Add(newVersion);

        // Update tags if provided
        if (updateModel.Tags is not null)
        {
            // Remove existing tags for this content
            var existingTags = await DbContext.Value.ContentTags.Where(ct => ct.ContentId == existing.Id).ToListAsync();
            DbContext.Value.ContentTags.RemoveRange(existingTags);

            // Add new tags
            foreach (var tagModel in updateModel.Tags)
            {
                var tag = await GetOrCreateTag(tagModel);
                var tagItem = new TagItem
                {
                    Id = Guid.NewGuid(),
                    ContentId = newVersion.Id,
                    TagId = tag.Id,
                    EmberUserId = userId,
                    ConfidenceRate = tagModel.ConfidenceRate,
                    IsPrivate = tag.IsPrivate
                };
                DbContext.Value.ContentTags.Add(tagItem);
            }
        }

        // Update collections if provided
        if (updateModel.CollectionIds is not null)
        {
            // Remove existing collection items
            var existingCollectionItems = await DbContext.Value.CollectionItems.Where(ci => ci.ContentId == existing.Id).ToListAsync();
            DbContext.Value.CollectionItems.RemoveRange(existingCollectionItems);

            // Add to new collections
            foreach (var collectionId in updateModel.CollectionIds)
            {
                var collectionItem = new ContentCollectionItem
                {
                    Id = Guid.NewGuid(),
                    CollectionId = collectionId,
                    ContentId = newVersion.Id,
                    OrderIndex = 0,
                    AddedAt = DateTimeOffset.UtcNow
                };
                DbContext.Value.CollectionItems.Add(collectionItem);
            }
        }

        await DbContext.Value.SaveChangesAsync();

        return new ContentModel
        {
            Id = newVersion.Id,
            Identifier = newVersion.Identifier,
            ParentContentId = newVersion.ParentContentId,
            ContentTypeId = newVersion.ContentTypeId,
            ContentFormatId = newVersion.ContentFormatId,
            ContentVisibilityId = newVersion.ContentVisibilityId,
            VisibilityCriteria = newVersion.VisibilityCriteria,
            Title = newVersion.Title,
            Data = newVersion.Data,
            EmberUserId = newVersion.EmberUserId,
            CreatedAt = newVersion.CreatedAt
        };
    }

    public async Task DeactivateContent(Guid contentId)
    {
        await using var processLog = LogHelper.Value.BeginLogScope<ProcessLog, ProcessLogArgs>(new ProcessLogArgs($"{nameof(KnowledgeService)}.{nameof(DeactivateContent)}", contentId));

        var content = await DbContext.Value.Contents.FindAsync(contentId);
        if (content == null || !content.IsActive)
        {
            throw new KeyNotFoundException($"Active content with ID {contentId} not found.");
        }

        var userId = RequestLogContext.Value.UserId ?? throw new InvalidOperationException("User ID not found in request context");
        if (content.EmberUserId != userId)
        {
            throw new UnauthorizedAccessException("Cannot deactivate content owned by another user.");
        }

        // In versioning system, "delete" means deactivate
        content.IsActive = false;
        content.RemovedTime = DateTimeOffset.UtcNow;

        await DbContext.Value.SaveChangesAsync();
    }

    private async Task<Tag> GetOrCreateTag(TagModel tagModel)
    {
        Tag tag;
        if (tagModel.Id != Guid.Empty)
        {
            tag = await DbContext.Value.Tags.FindAsync(tagModel.Id) ?? throw new KeyNotFoundException($"Tag with ID {tagModel.Id} not found.");
        }
        else if (!string.IsNullOrWhiteSpace(tagModel.Name))
        {
            tag = await DbContext.Value.Tags.FirstOrDefaultAsync(t => t.Name == tagModel.Name);
            if (tag == null)
            {
                tag = new Tag
                {
                    Id = Guid.NewGuid(),
                    Name = tagModel.Name,
                    IsPrivate = tagModel.IsPrivate,
                    HasConfidenceRate = tagModel.HasConfidenceRate,
                    EmberUserId = RequestLogContext.Value.UserId
                };
                DbContext.Value.Tags.Add(tag);
            }
        }
        else
        {
            throw new ArgumentException("Tag must have either an ID or a name.");
        }
        return tag;
    }

    public async Task<TagModel> CreateTag(TagCreateModel createModel)
    {
        await using var processLog = LogHelper.Value.BeginLogScope<ProcessLog, ProcessLogArgs>(new ProcessLogArgs($"{nameof(KnowledgeService)}.{nameof(CreateTag)}", createModel));

        var userId = RequestLogContext.Value.UserId ?? throw new InvalidOperationException("User ID not found in request context");
        var tag = new Tag
        {
            Id = Guid.NewGuid(),
            Name = createModel.Name,
            IsPrivate = createModel.IsPrivate,
            HasConfidenceRate = createModel.HasConfidenceRate,
            EmberUserId = userId
        };

        DbContext.Value.Tags.Add(tag);
        await DbContext.Value.SaveChangesAsync();

        return new TagModel
        {
            Id = tag.Id,
            Name = tag.Name,
            IsPrivate = tag.IsPrivate,
            HasConfidenceRate = tag.HasConfidenceRate
        };
    }

    public async Task<TagModel> UpdateTag(Guid tagId, TagUpdateModel updateModel)
    {
        await using var processLog = LogHelper.Value.BeginLogScope<ProcessLog, ProcessLogArgs>(new ProcessLogArgs($"{nameof(KnowledgeService)}.{nameof(UpdateTag)}", new { tagId, updateModel }));

        var tag = await DbContext.Value.Tags.FindAsync(tagId);
        if (tag == null)
        {
            throw new KeyNotFoundException($"Tag with ID {tagId} not found.");
        }

        var userId = RequestLogContext.Value.UserId ?? throw new InvalidOperationException("User ID not found in request context");
        if (tag.EmberUserId != userId)
        {
            throw new UnauthorizedAccessException("Cannot modify tag owned by another user.");
        }

        tag.Name = updateModel.Name ?? tag.Name;
        tag.IsPrivate = updateModel.IsPrivate;
        tag.HasConfidenceRate = updateModel.HasConfidenceRate;

        await DbContext.Value.SaveChangesAsync();

        return new TagModel
        {
            Id = tag.Id,
            Name = tag.Name,
            IsPrivate = tag.IsPrivate,
            HasConfidenceRate = tag.HasConfidenceRate
        };
    }

    public async Task<IEnumerable<TagModel>> GetTags()
    {
        var userId = RequestLogContext.Value.UserId;
        var tags = await DbContext.Value.Tags
            .Where(t => !t.IsPrivate || t.EmberUserId == userId)
            .Select(t => new TagModel
            {
                Id = t.Id,
                Name = t.Name,
                IsPrivate = t.IsPrivate,
                HasConfidenceRate = t.HasConfidenceRate
            })
            .ToListAsync();

        return tags;
    }

    public async Task<TagModel?> GetTag(Guid tagId)
    {
        var userId = RequestLogContext.Value.UserId;
        var tag = await DbContext.Value.Tags
            .FirstOrDefaultAsync(t => t.Id == tagId && (!t.IsPrivate || t.EmberUserId == userId));

        if (tag == null) return null;

        return new TagModel
        {
            Id = tag.Id,
            Name = tag.Name,
            IsPrivate = tag.IsPrivate,
            HasConfidenceRate = tag.HasConfidenceRate
        };
    }

    public async Task DeleteTag(Guid tagId)
    {
        await using var processLog = LogHelper.Value.BeginLogScope<ProcessLog, ProcessLogArgs>(new ProcessLogArgs($"{nameof(KnowledgeService)}.{nameof(DeleteTag)}", tagId));

        var tag = await DbContext.Value.Tags.FindAsync(tagId);
        if (tag == null)
        {
            throw new KeyNotFoundException($"Tag with ID {tagId} not found.");
        }

        var userId = RequestLogContext.Value.UserId;
        if (tag.EmberUserId != userId)
        {
            throw new UnauthorizedAccessException("Cannot delete tag owned by another user.");
        }

        DbContext.Value.Tags.Remove(tag);
        await DbContext.Value.SaveChangesAsync();
    }

    public async Task<CollectionModel> CreateCollection(CollectionCreateModel createModel)
    {
        await using var processLog = LogHelper.Value.BeginLogScope<ProcessLog, ProcessLogArgs>(new ProcessLogArgs($"{nameof(KnowledgeService)}.{nameof(CreateCollection)}", createModel));

        var userId = RequestLogContext.Value.UserId ?? throw new InvalidOperationException("User ID not found in request context");
        var collection = new ContentCollection
        {
            Id = Guid.NewGuid(),
            Name = createModel.Name,
            Description = createModel.Description,
            EmberUserId = userId,
            CreatedAt = DateTimeOffset.UtcNow
        };

        DbContext.Value.Collections.Add(collection);
        await DbContext.Value.SaveChangesAsync();

        return new CollectionModel
        {
            Id = collection.Id,
            Name = collection.Name,
            Description = collection.Description,
            EmberUserId = collection.EmberUserId,
            CreatedAt = collection.CreatedAt
        };
    }

    public async Task<CollectionModel> UpdateCollection(Guid collectionId, CollectionUpdateModel updateModel)
    {
        await using var processLog = LogHelper.Value.BeginLogScope<ProcessLog, ProcessLogArgs>(new ProcessLogArgs($"{nameof(KnowledgeService)}.{nameof(UpdateCollection)}", new { collectionId, updateModel }));

        var collection = await DbContext.Value.Collections.FindAsync(collectionId);
        if (collection == null)
        {
            throw new KeyNotFoundException($"Collection with ID {collectionId} not found.");
        }

        var userId = RequestLogContext.Value.UserId ?? throw new InvalidOperationException("User ID not found in request context");
        if (collection.EmberUserId != userId)
        {
            throw new UnauthorizedAccessException("Cannot modify collection owned by another user.");
        }

        collection.Name = updateModel.Name ?? collection.Name;
        collection.Description = updateModel.Description ?? collection.Description;

        await DbContext.Value.SaveChangesAsync();

        return new CollectionModel
        {
            Id = collection.Id,
            Name = collection.Name,
            Description = collection.Description,
            EmberUserId = collection.EmberUserId,
            CreatedAt = collection.CreatedAt
        };
    }

    public async Task<IEnumerable<CollectionModel>> GetCollections()
    {
        var userId = RequestLogContext.Value.UserId;
        var collections = await DbContext.Value.Collections
            .Where(c => c.EmberUserId == userId)
            .Include(c => c.EmberUser)
            .Select(c => new CollectionModel
            {
                Id = c.Id,
                Name = c.Name,
                Description = c.Description,
                EmberUserId = c.EmberUserId,
                CreatedAt = c.CreatedAt
            })
            .ToListAsync();

        return collections;
    }

    public async Task<CollectionModel?> GetCollection(Guid collectionId)
    {
        var userId = RequestLogContext.Value.UserId;
        var collection = await DbContext.Value.Collections
            .Include(c => c.EmberUser)
            .Include(c => c.CollectionItems)
            .ThenInclude(ci => ci.Content)
            .FirstOrDefaultAsync(c => c.Id == collectionId && c.EmberUserId == userId);

        if (collection == null) return null;

        var items = collection.CollectionItems?
            .OrderBy(ci => ci.OrderIndex)
            .Select(ci => new CollectionItemModel
            {
                Id = ci.Id,
                ContentId = ci.ContentId,
                CollectionId = ci.CollectionId,
                OrderIndex = ci.OrderIndex,
                AddedAt = ci.AddedAt,
                Content = ci.Content != null ? new ContentModel
                {
                    Id = ci.Content.Id,
                    Identifier = ci.Content.Identifier,
                    ParentContentId = ci.Content.ParentContentId,
                    ContentTypeId = ci.Content.ContentTypeId,
                    ContentFormatId = ci.Content.ContentFormatId,
                    ContentVisibilityId = ci.Content.ContentVisibilityId,
                    VisibilityCriteria = ci.Content.VisibilityCriteria,
                    Title = ci.Content.Title,
                    Data = ci.Content.Data,
                    EmberUserId = ci.Content.EmberUserId,
                    CreatedAt = ci.Content.CreatedAt
                } : null
            });

        return new CollectionModel
        {
            Id = collection.Id,
            Name = collection.Name,
            Description = collection.Description,
            EmberUserId = collection.EmberUserId,
            CreatedAt = collection.CreatedAt,
            Items = items
        };
    }

    public async Task AddContentToCollection(Guid collectionId, CollectionItemCreateModel itemModel)
    {
        await using var processLog = LogHelper.Value.BeginLogScope<ProcessLog, ProcessLogArgs>(new ProcessLogArgs($"{nameof(KnowledgeService)}.{nameof(AddContentToCollection)}", new { collectionId, itemModel }));

        var userId = RequestLogContext.Value.UserId;
        var collection = await DbContext.Value.Collections.FindAsync(collectionId);
        if (collection == null || collection.EmberUserId != userId)
        {
            throw new UnauthorizedAccessException("Collection not found or access denied.");
        }

        var collectionItem = new ContentCollectionItem
        {
            Id = Guid.NewGuid(),
            CollectionId = collectionId,
            ContentId = itemModel.ContentId,
            OrderIndex = itemModel.OrderIndex,
            AddedAt = DateTimeOffset.UtcNow
        };

        DbContext.Value.CollectionItems.Add(collectionItem);
        await DbContext.Value.SaveChangesAsync();
    }

    public async Task RemoveContentFromCollection(Guid collectionId, Guid contentId)
    {
        await using var processLog = LogHelper.Value.BeginLogScope<ProcessLog, ProcessLogArgs>(new ProcessLogArgs($"{nameof(KnowledgeService)}.{nameof(RemoveContentFromCollection)}", new { collectionId, contentId }));

        var userId = RequestLogContext.Value.UserId;
        var item = await DbContext.Value.CollectionItems
            .FirstOrDefaultAsync(ci => ci.CollectionId == collectionId && ci.ContentId == contentId &&
                                     DbContext.Value.Collections.Any(c => c.Id == collectionId && c.EmberUserId == userId));

        if (item != null)
        {
            DbContext.Value.CollectionItems.Remove(item);
            await DbContext.Value.SaveChangesAsync();
        }
    }

    public async Task DeleteCollection(Guid collectionId)
    {
        await using var processLog = LogHelper.Value.BeginLogScope<ProcessLog, ProcessLogArgs>(new ProcessLogArgs($"{nameof(KnowledgeService)}.{nameof(DeleteCollection)}", collectionId));

        var userId = RequestLogContext.Value.UserId ?? throw new InvalidOperationException("User ID not found in request context");
        var collection = await DbContext.Value.Collections.FindAsync(collectionId);
        if (collection == null || collection.EmberUserId != userId)
        {
            throw new UnauthorizedAccessException("Collection not found or access denied.");
        }

        DbContext.Value.Collections.Remove(collection);
        await DbContext.Value.SaveChangesAsync();
    }
}
