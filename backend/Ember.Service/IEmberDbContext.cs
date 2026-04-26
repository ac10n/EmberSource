using Ember.Domain.Data;
using Ember.Domain.EmberEntities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Ember.Service
{
    public interface IEmberDbContext
    {
        public DbSet<RefreshToken> RefreshTokens { get; }
        public DbSet<Invitation> Invitations { get; }

        public DbSet<Content> Contents { get; set; }
        public DbSet<ContentFormat> ContentFormats { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<TagItem> ContentTags { get; set; }
        public DbSet<ContentType> ContentTypes { get; set; }
        public DbSet<ContentVisibility> ContentVisibilities { get; set; }
        public DbSet<RelatedContent> RelatedContents { get; set; }
        public DbSet<RelatedContentType> RelatedContentTypes { get; set; }
        public DbSet<Collection> Collections { get; set; }
        public DbSet<CollectionItem> CollectionItems { get; set; }
        public DbSet<ContentInteraction> ContentInteractions { get; set; }

        public DbSet<BadgeDefinition> BadgeDefinitions { get; set; }

        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}
