using Ember.WebServer.Areas.Knowledge.Entities;

namespace Ember.WebServer.Data;

public static class KnownUsers
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
        ConcurrencyStamp = "a5405995-3314-4821-be2f-17ddabd54fbf",
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
        ConcurrencyStamp = "2aaf7da4-ce97-4efb-a069-55ff3c131465",
    };

    public static IEnumerable<EmberUser> AllKnownUsers()
    {
        yield return DefaultSystemUser;
        yield return Founder;
    }
}

public static class KnownRoles
{
    public static EmberRole Root = new() { Id = Guid.Parse("f883569f-2494-4328-95a9-e76a2b6ff143"), Name = "Root", NormalizedName = "Root" };
    public static EmberRole Owner = new() { Id = Guid.Parse("5e4b79a1-afe1-4e79-a4e1-ee1b86e4ce33"), Name = "Owner", NormalizedName = "OWNER" };
    public static EmberRole Administrator = new() { Id = Guid.Parse("5e848956-8a01-4816-be8d-d6a6b7091cf3"), Name = "Administrator", NormalizedName = "ADMINISTRATOR" };
    public static EmberRole Creator = new() { Id = Guid.Parse("b373f8c6-b1b9-4097-98e1-46a881e5484d"), Name = "Creator", NormalizedName = "CREATOR" };
    public static EmberRole User = new() { Id = Guid.Parse("a03c7114-423e-47e1-a280-6ba824c25828"), Name = "User", NormalizedName = "USER" };

    public static IEnumerable<EmberRole> AllKnownRoles()
    {
        yield return Root;
        yield return Owner;
        yield return Administrator;
        yield return Creator;
        yield return User;
    }
}

public static class KnownUserRoles
{
    public static EmberUserRole SystemRoot = new()
    {
        Id = Guid.Parse("43510e33-41e3-45bb-96c3-be52af6b9c3b"),
        UserId = KnownUsers.DefaultSystemUser.Id,
        RoleId = KnownRoles.Root.Id,
        PlatformSectionId = null,
    };
    public static EmberUserRole FounderOwner = new()
    {
        Id = Guid.Parse("072c2bb6-f216-4c07-8e4c-f6b3abb05770"),
        UserId = KnownUsers.Founder.Id,
        RoleId = KnownRoles.Owner.Id,
        PlatformSectionId = null,
    };

    public static IEnumerable<EmberUserRole> AllKnownUserRoles()
    {
        yield return SystemRoot;
        yield return FounderOwner;
    }
}

public static class KnownPlatformSections
{
    public static readonly PlatformSection EmberFoundation = new()
    {
        Id = Guid.Parse("d04c1e89-a4c8-450b-8608-84f35415fd39"),
        Name = "Ember Foundation",
        Description = "The Ember Foundation, the non-profit organization that drives all other Ember projects.",
        Url = "/",
        CreatorUserId = KnownUsers.Founder.Id,
        ParentSectionId = null,
        InheritRoles = false,
        FinancialModelId = KnownFinancialModels.NonProfit.Id
    };

    public static IEnumerable<PlatformSection> AllKnownPlatformSections()
    {
        yield return EmberFoundation;
    }
}

public static class KnownContent
{
    public static readonly Content WelcomeArticle = new()
    {
        Id = Guid.Parse("3173c43b-b415-4d45-a002-7c7d7253bf1b"),
        Identifier = Guid.Parse("3fee5a19-69e4-4c65-81fe-8d9de9c11539"),
        Version = 1,
        ParentContentId = null,
        ContentTypeId = ContentTypes.Paragraph,
        ContentFormatId = ContentFormats.Markdown,
        FormatVersion = 1,
        Title = "Welcome to Ember",
        Data =
@"# Welcome to Ember!

Ember is an attempt to give users ownership of their online activities.
An online service that does not seek profit, would give users tools they need to find the data they want,
interact with the platform in ways that make sense to their use cases,
keep the user in control, not at the mercy of intentionally limited UX and engagement-maximizing algorithms.

Transparency builds dependable trust, amongst users, and between users and the platform.
With time, we can create tools that provide users with a measure of trustworthiness of content and its creators.
These measures should reflect each user's values and priorities, not those imposed by a centralized authority.

The premise is that a confused user is more likely to make decisions that are not in their best interest.

We'll start with the simplest tools: a place you can write and share pieces of content.
With that, conversations around Ember development can be hosted on the platform itself.

Ember is an attempt to bring back how trust shaped human interactions in small communities,
but we lost that when we moved to the global village, and platforms that seek profit at the expense societal good.

Much more details to be shared as we build more features.
",
        CreatedByUserId = KnownUsers.Founder.Id,
        CreatedAt = DateTimeOffset.Parse("2026-01-31T14:44:00"),
        IsActive = true,
        ContentVisibilityId = ContentVisibilities.Public
    };

    public static IEnumerable<Content> AllKnownContents()
    {
        yield return WelcomeArticle;
    }
}
