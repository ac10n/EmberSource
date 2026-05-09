using Ember.Domain.Data;
using Ember.Domain.EmberEntities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Ember.Infrastructure.EntityConfigurations;

public sealed class ContentConfiguration : IEntityTypeConfiguration<Content>
{
    public void Configure(EntityTypeBuilder<Content> builder)
    {
        builder.ToTable("Contents", "DOC");

        builder.HasIndex(x => x.ContentFormatId);
        builder.HasIndex(x => x.ContentTypeId);
        builder.HasIndex(x => x.ContentVisibilityId);
        builder.HasIndex(x => x.EmberUserId);
        builder.HasIndex(x => x.ParentContentId);

        builder.HasOne(x => x.ContentFormat)
            .WithMany()
            .HasForeignKey(x => x.ContentFormatId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(x => x.ContentType)
            .WithMany()
            .HasForeignKey(x => x.ContentTypeId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(x => x.ContentVisibility)
            .WithMany()
            .HasForeignKey(x => x.ContentVisibilityId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(x => x.EmberUser)
            .WithMany()
            .HasForeignKey(x => x.EmberUserId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(x => x.ParentContent)
            .WithMany(x => x.ChildContents)
            .HasForeignKey(x => x.ParentContentId);
    }
}

public sealed class ContentCollectionConfiguration : IEntityTypeConfiguration<ContentCollection>
{
    public void Configure(EntityTypeBuilder<ContentCollection> builder)
    {
        builder.ToTable("Collections", "DOC");

        builder.HasIndex(x => x.EmberUserId);

        builder.HasOne(x => x.EmberUser)
            .WithMany()
            .HasForeignKey(x => x.EmberUserId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}

public sealed class ContentCollectionItemConfiguration : IEntityTypeConfiguration<ContentCollectionItem>
{
    public void Configure(EntityTypeBuilder<ContentCollectionItem> builder)
    {
        builder.ToTable("CollectionItems", "DOC");

        builder.HasIndex(x => x.CollectionId);
        builder.HasIndex(x => x.ContentId);

        builder.HasOne(x => x.Collection)
            .WithMany(x => x.CollectionItems)
            .HasForeignKey(x => x.CollectionId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(x => x.Content)
            .WithMany()
            .HasForeignKey(x => x.ContentId);
    }
}

public sealed class ContentFormatConfiguration : IEntityTypeConfiguration<ContentFormat>
{
    public void Configure(EntityTypeBuilder<ContentFormat> builder)
    {
        builder.ToTable("ContentFormats", "DOC");
    }
}

public sealed class ContentInteractionConfiguration : IEntityTypeConfiguration<ContentInteraction>
{
    public void Configure(EntityTypeBuilder<ContentInteraction> builder)
    {
        builder.ToTable("ContentInteractions", "DOC");

        builder.HasIndex(x => x.ContentId);
        builder.HasIndex(x => x.UserId);

        builder.HasOne(x => x.Content)
            .WithMany()
            .HasForeignKey(x => x.ContentId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(x => x.User)
            .WithMany()
            .HasForeignKey(x => x.UserId);
    }
}

public sealed class ContentTypeConfiguration : IEntityTypeConfiguration<ContentType>
{
    public void Configure(EntityTypeBuilder<ContentType> builder)
    {
        builder.ToTable("ContentTypes", "DOC");
    }
}

public sealed class ContentVisibilityConfiguration : IEntityTypeConfiguration<ContentVisibility>
{
    public void Configure(EntityTypeBuilder<ContentVisibility> builder)
    {
        builder.ToTable("ContentVisibilities", "DOC");
    }
}

public sealed class DataOwnershipConfiguration : IEntityTypeConfiguration<DataOwnership>
{
    public void Configure(EntityTypeBuilder<DataOwnership> builder)
    {
        builder.ToTable("DataOwnerships", "DOC");
    }
}

public sealed class FinancialModelConfiguration : IEntityTypeConfiguration<FinancialModel>
{
    public void Configure(EntityTypeBuilder<FinancialModel> builder)
    {
        builder.ToTable("FinancialModels", "DOC");
    }
}

public sealed class PlatformSectionConfiguration : IEntityTypeConfiguration<PlatformSection>
{
    public void Configure(EntityTypeBuilder<PlatformSection> builder)
    {
        builder.ToTable("PlatformSections", "DOC");

        builder.HasIndex(x => x.CreatorUserId);
        builder.HasIndex(x => x.FinancialModelId);
        builder.HasIndex(x => x.ParentSectionId);

        builder.HasOne(x => x.CreatorUser)
            .WithMany()
            .HasForeignKey(x => x.CreatorUserId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(x => x.FinancialModel)
            .WithMany()
            .HasForeignKey(x => x.FinancialModelId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(x => x.ParentPlatformSection)
            .WithMany()
            .HasForeignKey(x => x.ParentSectionId);
    }
}

public sealed class RelatedContentConfiguration : IEntityTypeConfiguration<RelatedContent>
{
    public void Configure(EntityTypeBuilder<RelatedContent> builder)
    {
        builder.ToTable("RelatedContents", "DOC");

        builder.HasIndex(x => x.ContentId);
        builder.HasIndex(x => x.RelatedContentId);
        builder.HasIndex(x => x.RelatedContentTypeId);

        builder.HasOne(x => x.Content)
            .WithMany()
            .HasForeignKey(x => x.ContentId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(x => x.RelatedContentItem)
            .WithMany()
            .HasForeignKey(x => x.RelatedContentId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(x => x.RelatedContentType)
            .WithMany()
            .HasForeignKey(x => x.RelatedContentTypeId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}

public sealed class RelatedContentTypeConfiguration : IEntityTypeConfiguration<RelatedContentType>
{
    public void Configure(EntityTypeBuilder<RelatedContentType> builder)
    {
        builder.ToTable("RelatedContentTypes", "DOC");
    }
}

public sealed class TagConfiguration : IEntityTypeConfiguration<Tag>
{
    public void Configure(EntityTypeBuilder<Tag> builder)
    {
        builder.ToTable("Tags", "DOC");

        builder.HasIndex(x => x.EmberUserId);

        builder.HasOne(x => x.EmberUser)
            .WithMany()
            .HasForeignKey(x => x.EmberUserId);
    }
}

public sealed class TagItemConfiguration : IEntityTypeConfiguration<TagItem>
{
    public void Configure(EntityTypeBuilder<TagItem> builder)
    {
        builder.ToTable("ContentTags", "DOC");

        builder.HasIndex(x => x.CollectionId);
        builder.HasIndex(x => x.ContentId);
        builder.HasIndex(x => x.EmberUserId);
        builder.HasIndex(x => x.TagId);

        builder.HasOne(x => x.Collection)
            .WithMany()
            .HasForeignKey(x => x.CollectionId);

        builder.HasOne(x => x.Content)
            .WithMany()
            .HasForeignKey(x => x.ContentId);

        builder.HasOne(x => x.EmberUser)
            .WithMany()
            .HasForeignKey(x => x.EmberUserId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(x => x.Tag)
            .WithMany()
            .HasForeignKey(x => x.TagId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
