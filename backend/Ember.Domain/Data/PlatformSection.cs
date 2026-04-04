namespace Ember.Domain.Data;

public class FinancialModel
{
    public required int Id { get; set; }
    public required string Name { get; set; }
    public required string Description { get; set; }
}

public static class KnownFinancialModels
{
    public static readonly FinancialModel NonProfit = new()
    {
        Id = 1,
        Name = "Non-Profit",
        Description = "A financial model where the organization does not distribute profits to owners or shareholders."
    };

    public static readonly FinancialModel ForProfit = new()
    {
        Id = 2,
        Name = "For-Profit",
        Description = "A financial model where the organization aims to generate profit for its owners or shareholders."
    };

    public static readonly FinancialModel[] AllModels = [
        NonProfit,
        ForProfit,
    ];
}

public class PlatformSection
{
    public required Guid Id { get; set; }
    public required string Name { get; set; }

    public Guid? ParentSectionId { get; set; }
    public PlatformSection? ParentPlatformSection { get; set; }

    public required string Description { get; set; }
    public required string Url { get; set; }

    public Guid CreatorUserId { get; set; }
    public EmberUser? CreatorUser { get; set; }

    public bool InheritRoles { get; set; }

    public required int FinancialModelId { get; set; }
    public FinancialModel? FinancialModel { get; set; }
}
