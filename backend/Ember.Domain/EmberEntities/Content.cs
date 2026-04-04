using System.ComponentModel.DataAnnotations.Schema;
using Ember.Domain.Data;

namespace Ember.Domain.EmberEntities;

public class Content
{
    public Guid Id { get; set; }
    /// <summary>
    /// with Id and version we can track the content history, and also allow users to reference specific versions of the content.
    /// So we can select the latest version of the content by filtering on the Id and is active and also we can select specific version of the content by filtering on the Id and version number.
    /// </summary>
    public required Guid Identifier { get; set; }
    /// <summary>
    /// Gets or sets the version number for the current instance.
    /// </summary>
    public required int Version { get; set; } = 1;

    public Guid? ParentContentId { get; set; }
    public Content? ParentContent { get; set; }
    public ICollection<Content>? ChildContents { get; set; }

    public required ContentTypes ContentTypeId { get; set; }
    public ContentType? ContentType { get; set; }

    public required ContentFormatEnum ContentFormatId { get; set; }
    public ContentFormat? ContentFormat { get; set; }
    /// <summary>
    /// The version of editor or format used to create the content. This allows us to handle changes in the content structure or formatting rules over time. 
    /// When we update the editor or change the content format, we can increment the FormatVersion to indicate that this content uses a new format.
    /// This way, when we retrieve and render the content, we can check the FormatVersion and apply the appropriate rendering logic based on the version of the format used to create it.
    /// </summary>
    public required int FormatVersion { get; set; } = 1;

    public string? Title { get; set; }
    /// <summary>
    /// base on content type and content format, we can determine how to interpret the data field. 
    /// For example, if the content type is paragraph and the content format is markdown, then we know the data field contains markdown text that represents a paragraph. 
    /// If the content type is image and the content format is plain text, then we know the data field contains a URL or file path to the image.
    /// </summary>
    public required string Data { get; set; }

    public Guid EmberUserId { get; set; }
    public EmberUser? EmberUser { get; set; }

    public DateTimeOffset CreatedAt { get; set; }
    /// <summary>
    /// Gets or sets a value indicating whether the object is active.
    /// when is active is false, it means the content is removed but we keep the record for history and reference.
    /// also it can be an old version of the content, and we can use it to track the content history and also allow users to reference specific versions of the content.
    /// </summary>
    public bool IsActive { get; set; }
    /// <summary>
    /// when we update the content, we can set the current version IsActive to false and create a new version with IsActive true
    /// then set the removed time for the old version
    /// </summary>
    public DateTimeOffset? RemovedTime { get; set; }
    /// <summary>
    /// who can see the content
    /// </summary>
    public required ContentVisibilityEnum ContentVisibilityId { get; set; }
    public ContentVisibility ContentVisibility { get; set; } = null!;
    /// <summary>
    /// Gets or sets the criteria used to determine the visibility of the associated element.
    /// if the visibility is set to VisibleByCriteria, this field can contain a JSON string or any structured data that defines the specific conditions or rules that determine who can see the content.
    /// if the visibility is set to ScheduledRollout, this field can contain the schedule information such as start time and end time for the content visibility.
    /// if the visibility is set to VisibleToSpecificPeople, this field can contain the list of user ids or group ids who can see the content.
    /// </summary>
    public string? VisibilityCriteria { get; set; }
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

/// <summary>
/// Create a table for enum ContentFormat, this table is just for better readability and maintainability, it allows us to easily manage and update the enum values without having to change the code.
/// </summary>
public class ContentFormat
{
    public ContentFormatEnum Id { get; set; }
    public required string Name { get; set; }
}

public enum ContentFormatEnum
{
    Markdown = 1,
    RichText = 2,
    PlainText = 3,
    Html = 4,
}

/// <summary>
/// we create a table for enums this table is just for better readability and maintainability, it allows us to easily manage and update the enum values without having to change the code.
/// </summary>
public class ContentVisibility
{
    public ContentVisibilityEnum Id { get; set; }
    public string? Name { get; set; }
}

public enum ContentVisibilityEnum
{
    Public = 1,
    Private,
    VisibleToSpecificPeople,
    VisibleByCriteria,
    ScheduledRollout,
}
