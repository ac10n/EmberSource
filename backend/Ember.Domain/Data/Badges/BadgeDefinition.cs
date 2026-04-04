namespace Ember.Domain.Data;

/// <summary>
/// system gives users badges based on their interactions, achievements, or contributions within the platform. 
/// Each badge has a definition that outlines its characteristics and criteria for awarding it to users. 
/// we calc based on some caculation and then we give the badge to the user with a value if it's numeric or boolean if it's not numeric
/// </summary>
public class BadgeDefinition
{
    public Guid Id { get; set; }
    public required string Name { get; set; }
    public bool IsNumeric { get; set; }
    public bool IsFractional { get; set; }
    public decimal MinValue { get; set; }
    public decimal MaxValue { get; set; }
    public decimal? DefaultValue { get; set; }
}
