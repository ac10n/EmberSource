using Ember.WebServer.Areas.Knowledge.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Ember.WebServer.Data;

public class EmberDbContext(DbContextOptions<EmberDbContext> options)
    : IdentityDbContext<EmberUser, EmberRole, Guid>(options)
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

        builder.Entity<DataOwnership>()
            .HasData(Enum.GetValues<DataOwnershipType>()
                .Select(ownershipType => new DataOwnership
                {
                    Id = ownershipType,
                    Name = ownershipType.ToString()
                })
            );

        builder.Entity<EmberRole>().HasData(
            new EmberRole { Id = Guid.NewGuid(), Name = "Root", NormalizedName = "Root" },
            new EmberRole { Id = Guid.NewGuid(), Name = "Owner", NormalizedName = "OWNER" },
            new EmberRole { Id = Guid.NewGuid(), Name = "Administrator", NormalizedName = "ADMINISTRATOR" },
            new EmberRole { Id = Guid.NewGuid(), Name = "Creator", NormalizedName = "CREATOR" },
            new EmberRole { Id = Guid.NewGuid(), Name = "User", NormalizedName = "USER" }
        );

        builder.Entity<EmberUser>().HasData(SystemUsers.DefaultSystemUser);
        builder.Entity<EmberUser>().HasData(SystemUsers.Founder);

        builder.Entity<EmberUserRole>().HasData(
            new
            {
                UserId = SystemUsers.DefaultSystemUser.Id,
                RoleId = Guid.Parse("Root"),
                PlatformSectionId = Guid.Empty
            },
            new
            {
                UserId = SystemUsers.Founder.Id,
                RoleId = Guid.Parse("Owner"),
                PlatformSectionId = Guid.Empty
            }
        );

        builder.Entity<FinancialModel>().HasData(KnownFinancialModels.AllModels);

        builder.Entity<PlatformSection>().HasData(
            new PlatformSection
            {
                Id = Guid.NewGuid(),
                Name = "Ember Foundation",
                CreatorUser = SystemUsers.Founder,
                Description = "The Ember Foundation, the non-profit organization that drives all other Ember projects.",
                Url = "/",
                CreatorUserId = SystemUsers.DefaultSystemUser.Id,
                ParentSectionId = null,
                InheritRoles = false,
                FinancialModelId = KnownFinancialModels.NonProfit.Id
            }
        );
    }
}
