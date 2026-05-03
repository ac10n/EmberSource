using Ember.Domain.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Ember.Infrastructure.EntityConfigurations;

public sealed class BadgeDefinitionConfiguration : IEntityTypeConfiguration<BadgeDefinition>
{
    public void Configure(EntityTypeBuilder<BadgeDefinition> builder)
    {
        builder.ToTable("BadgeDefinitions", "USR");
    }
}

public sealed class InvitationConfiguration : IEntityTypeConfiguration<Invitation>
{
    public void Configure(EntityTypeBuilder<Invitation> builder)
    {
        builder.ToTable("Invitations", "USR");

        builder.HasIndex(x => x.InvitedByUserId);
        builder.HasIndex(x => x.InviteCode).IsUnique();

        builder.HasOne(x => x.InvitedByUser)
            .WithMany()
            .HasForeignKey(x => x.InvitedByUserId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(x => x.AcceptedByUser)
            .WithMany()
            .HasForeignKey(x => x.AcceptedByUserId);
    }
}

public sealed class TestimonialConfiguration : IEntityTypeConfiguration<Testimonial>
{
    public void Configure(EntityTypeBuilder<Testimonial> builder)
    {
        builder.ToTable("Testimonials", "DOC");

        builder.HasIndex(x => x.BadgeDefinitionId);
        builder.HasIndex(x => x.ByEmberUserId);
        builder.HasIndex(x => x.ForEmberUserId);

        builder.HasOne(x => x.BadgeDefinition)
            .WithMany()
            .HasForeignKey(x => x.BadgeDefinitionId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(x => x.ByEmberUser)
            .WithMany()
            .HasForeignKey(x => x.ByEmberUserId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(x => x.ForEmberUser)
            .WithMany()
            .HasForeignKey(x => x.ForEmberUserId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}

public sealed class UserBadgeValueConfiguration : IEntityTypeConfiguration<UserBadgeValue>
{
    public void Configure(EntityTypeBuilder<UserBadgeValue> builder)
    {
        builder.ToTable("UserBadgeValues", "USR");

        builder.HasIndex(x => x.BadgeDefinitionId);
        builder.HasIndex(x => x.EmberUserId);

        builder.HasOne(x => x.BadgeDefinition)
            .WithMany()
            .HasForeignKey(x => x.BadgeDefinitionId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(x => x.EmberUser)
            .WithMany()
            .HasForeignKey(x => x.EmberUserId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
