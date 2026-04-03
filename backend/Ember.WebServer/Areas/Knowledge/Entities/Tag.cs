namespace Ember.WebServer.Areas.Knowledge.Entities;

public class Tag
{
    public Guid Id { get; set; }

    public string? Name { get; set; }
}

public class ContentTag
{
    public Guid Id { get; set; }

    public required Guid ContentId { get; set; }
    public Content? Content { get; set; }

    public required Guid TagId { get; set; }
    public Tag? Tag { get; set; }
}
