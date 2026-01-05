namespace Ember.WebServer.Data;

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
