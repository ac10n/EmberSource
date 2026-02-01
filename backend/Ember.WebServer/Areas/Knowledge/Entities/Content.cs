using System.ComponentModel.DataAnnotations.Schema;
using Ember.WebServer.Data;

namespace Ember.WebServer.Areas.Knowledge.Entities;

public class Content
{
    public Guid Id { get; set; }

    public required Guid Identifier { get; set; }
    public required int Version { get; set; } = 1;

    public Guid? ParentContentId { get; set; }
    public Content? ParentContent { get; set; }
    public ICollection<Content>? ChildContents { get; set; }

    public required ContentTypes ContentTypeId { get; set; }
    public ContentType? ContentType { get; set; }

    public required ContentFormats ContentFormatId { get; set; }
    public ContentFormat? ContentFormat { get; set; }

    public required int FormatVersion { get; set; } = 1;

    public string? Title { get; set; }
    public string? Data { get; set; }

    public Guid CreatedByUserId { get; set; }
    public EmberUser? CreatedByUser { get; set; }

    public DateTimeOffset CreatedAt { get; set; }
    public bool IsActive { get; set; }
    public DateTimeOffset? RemovedTime { get; set; }

    public required ContentVisibilities ContentVisibilityId { get; set; }
    public ContentVisibility? ContentVisibility { get; set; }
}

public class ContentType
{
    public ContentTypes Id { get; set; }

    public required string Name { get; set; }
}

public enum ContentTypes
{
    Paragraph = 1,
    Section,
    Article,
    Image,
    Video,
    Audio,
    Link,
    InteractiveElement,
    CodeSnippet,
    ExplorableDataset,
    Comment,
}

public class ContentFormat
{
    public ContentFormats Id { get; set; }
    public required string Name { get; set; }
}

public enum ContentFormats
{
    Markdown = 1,
}

public class ContentVisibility
{
    public ContentVisibilities Id { get; set; }
    public string? Name { get; set; }
}

public enum ContentVisibilities
{
    Public = 1,
    Private,
    VisibleToSpecificPeople,
    VisibleByCriteria,
    ScheduledRollout,
}
