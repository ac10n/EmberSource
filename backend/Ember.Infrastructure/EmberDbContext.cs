using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Ember.Domain.EmberEntities;
using Ember.Domain.Data;
using Ember.Service;

namespace Ember.Infrastructure;

public class EmberDbContext(DbContextOptions<EmberDbContext> options)
    : IdentityDbContext<EmberUser, EmberRole, Guid, IdentityUserClaim<Guid>, EmberUserRole, EmberUserLogin, EmberRoleClaim, EmberUserToken>(options), IEmberDbContext
{
    public DbSet<RefreshToken> RefreshTokens { get; set; }

    public DbSet<DataOwnership> DataOwnerships { get; set; }
    public DbSet<RequestLog> RequestLogs { get; set; }
    public DbSet<ResponseLog> ResponseLogs { get; set; }
    public DbSet<InteractionLog> InteractionLogs { get; set; }
    public DbSet<ActionLog> ActionLogs { get; set; }
    public DbSet<ProcessLog> ProcessLogs { get; set; }


    public DbSet<UserBadgeValue> UserBadgeValues { get; set; }
    public DbSet<BadgeDefinition> BadgeDefinitions { get; set; }
    public DbSet<Testimonial> Testimonials { get; set; }
    public DbSet<Invitation> Invitations { get; set; }


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

    public DbSet<FinancialModel> FinancialModels { get; set; }

    public DbSet<PlatformSection> PlatformSections { get; set; }



    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<RefreshToken>()
            .HasIndex(x => x.UserId);

        builder.Entity<RefreshToken>()
            .HasIndex(x => x.TokenHash)
            .IsUnique();

        builder.Entity<Invitation>()
            .HasIndex(i => i.InvitedByUserId);

        builder.Entity<Invitation>()
            .HasIndex(i => i.InviteCode)
            .IsUnique();

        builder.Entity<EmberUserRole>()
            .HasOne(ur => ur.User)
            .WithMany(u => u.UserRoles)
            .HasForeignKey(ur => ur.UserId);

        builder.Entity<EmberUserRole>()
            .HasOne(ur => ur.Role)
            .WithMany(r => r.UserRoles)
            .HasForeignKey(ur => ur.RoleId);
    }
}
