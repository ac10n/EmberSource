using Microsoft.AspNetCore.Identity;

namespace Ember.WebServer.Data;

public class EmberUser : IdentityUser<Guid>
{
    public int BirthYear { get; set; }
    public required string FullName { get; set; }
    public required string Jurisdiction { get; set; }
}

public static class SystemUsers
{
    public static readonly EmberUser DefaultSystemUser = new()
    {
        Id = Guid.Parse("6c0a35b2-23c4-44a3-8e13-161285b0f9db"),
        UserName = "System",
        NormalizedUserName = "SYSTEM",
        FullName = "System User",
        Jurisdiction = "Canada",
        EmailConfirmed = true,
        PhoneNumberConfirmed = true,
    };

    public static readonly EmberUser Founder = new()
    {
        Id = Guid.Parse("2f673ad9-6913-46d0-9b44-d517c5611c8c"),
        UserName = "ali",
        NormalizedUserName = "ALI",
        FullName = "Alireza Haghshenas",
        Jurisdiction = "Canada",
        EmailConfirmed = true,
        PhoneNumberConfirmed = true,
    };
}

public class EmberRole : IdentityRole<Guid>
{
}

public class EmberUserRole : IdentityUserRole<Guid>
{
    public required EmberUser User { get; set; }
    public required EmberRole Role { get; set; }

    public Guid PlatformSectionId { get; set; }
    public PlatformSection? PlatformSection { get; set; }
}
