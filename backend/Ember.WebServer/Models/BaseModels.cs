namespace Ember.WebServer.Models;

public abstract class ListRequestBase
{
    public int MaxCount { get; set; } = 100;
    public string? ContinuationToken { get; set; }
    public IEnumerable<string>? Sorting { get; set; }
}

public abstract class ListResponseBase
{
    public string? ContinuationToken { get; set; }
}
