using Ember.WebServer.Areas.Knowledge.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Ember.WebServer.Data;

public class EmberDbContext(DbContextOptions<EmberDbContext> options)
    : IdentityDbContext<EmberUser, EmberRole, Guid, IdentityUserClaim<Guid>, EmberUserRole, EmberUserLogin, EmberRoleClaim, EmberUserToken>(options)
{
    public DbSet<DataOwnership> LogOwnerships { get; set; }
    public DbSet<RequestLog> RequestLogs { get; set; }
    public DbSet<ResponseLog> ResponseLogs { get; set; }
    public DbSet<InteractionLog> InteractionLogs { get; set; }
    public DbSet<ActionLog> ActionLogs { get; set; }
    public DbSet<ProcessLog> ProcessLogs { get; set; }


    public DbSet<UserBadgeValue> UserBadgeValues { get; set; }
    public DbSet<BadgeDefinition> BadgeDefinitions { get; set; }
    public DbSet<Testimonial> Testimonials { get; set; }


    public DbSet<Content> Contents { get; set; }
    public DbSet<Tag> Tags { get; set; }
    public DbSet<ContentTag> ContentTags { get; set; }
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

        builder.Entity<EmberUserRole>()
            .HasOne(ur => ur.User)
            .WithMany(u => u.UserRoles)
            .HasForeignKey(ur => ur.UserId);

        builder.Entity<EmberUserRole>()
            .HasOne(ur => ur.Role)
            .WithMany(r => r.UserRoles)
            .HasForeignKey(ur => ur.RoleId);

        builder.Entity<DataOwnership>()
            .HasData(Enum.GetValues<DataOwnershipType>()
                .Select(ownershipType => new DataOwnership
                {
                    Id = ownershipType,
                    Name = ownershipType.ToString()
                })
            );

        builder.Entity<EmberRole>().HasData(KnownRoles.AllKnownRoles());
        builder.Entity<EmberUser>().HasData(KnownUsers.AllKnownUsers());
        builder.Entity<EmberUserRole>().HasData(KnownUserRoles.AllKnownUserRoles());
        builder.Entity<FinancialModel>().HasData(KnownFinancialModels.AllModels);
        builder.Entity<PlatformSection>().HasData(KnownPlatformSections.AllKnownPlatformSections());
    }
}
