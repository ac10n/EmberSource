using Ember.Domain.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Ember.Infrastructure.EntityConfigurations;

public sealed class EmberRoleConfiguration : IEntityTypeConfiguration<EmberRole>
{
    public void Configure(EntityTypeBuilder<EmberRole> builder)
    {
        builder.ToTable("AspNetRoles", "SSO");
    }
}

public sealed class EmberUserConfiguration : IEntityTypeConfiguration<EmberUser>
{
    public void Configure(EntityTypeBuilder<EmberUser> builder)
    {
        builder.ToTable("AspNetUsers", "SSO");
    }
}

public sealed class EmberUserClaimConfiguration : IEntityTypeConfiguration<IdentityUserClaim<Guid>>
{
    public void Configure(EntityTypeBuilder<IdentityUserClaim<Guid>> builder)
    {
        builder.ToTable("AspNetUserClaims", "SSO");
    }
}

public sealed class EmberRoleClaimConfiguration : IEntityTypeConfiguration<EmberRoleClaim>
{
    public void Configure(EntityTypeBuilder<EmberRoleClaim> builder)
    {
        builder.ToTable("AspNetRoleClaims", "SSO");
    }
}

public sealed class EmberUserLoginConfiguration : IEntityTypeConfiguration<EmberUserLogin>
{
    public void Configure(EntityTypeBuilder<EmberUserLogin> builder)
    {
        builder.ToTable("AspNetUserLogins", "SSO");
    }
}

public sealed class EmberUserRoleConfiguration : IEntityTypeConfiguration<EmberUserRole>
{
    public void Configure(EntityTypeBuilder<EmberUserRole> builder)
    {
        builder.ToTable("AspNetUserRoles", "SSO");

        builder.HasKey(x => new { x.UserId, x.RoleId });

        builder.HasIndex(x => x.PlatformSectionId);
        builder.HasIndex(x => x.RoleId);

        builder.HasOne(x => x.User)
            .WithMany(x => x.UserRoles)
            .HasForeignKey(x => x.UserId);

        builder.HasOne(x => x.Role)
            .WithMany(x => x.UserRoles)
            .HasForeignKey(x => x.RoleId);
    }
}

public sealed class EmberUserTokenConfiguration : IEntityTypeConfiguration<EmberUserToken>
{
    public void Configure(EntityTypeBuilder<EmberUserToken> builder)
    {
        builder.ToTable("AspNetUserTokens", "SSO");
    }
}

public sealed class RefreshTokenConfiguration : IEntityTypeConfiguration<RefreshToken>
{
    public void Configure(EntityTypeBuilder<RefreshToken> builder)
    {
        builder.ToTable("RefreshTokens", "SSO");
    }
}
