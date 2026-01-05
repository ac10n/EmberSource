namespace Ember.WebServer.Data;

public class DataOwnership
{
    public DataOwnershipType Id { get; set; }

    public required string Name { get; set; }
}

public interface IOwnedData
{
    public DataOwnershipType Owner { get; set; }
}

public enum DataOwnershipType
{
    System = 1,
    User = 2
}
