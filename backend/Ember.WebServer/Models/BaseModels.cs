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

public enum UpdateResultKind
{
    Success,
    Failure
}

public enum FailureReason
{
    Logical,
    Permission,
    Transient,
    Unknown,
}

public class UpdateResult<TModel>
{
    public UpdateResultKind Result { get; set; }
    public TModel? Data { get; set; }
    public FailureReason? Reason { get; set; }
}
