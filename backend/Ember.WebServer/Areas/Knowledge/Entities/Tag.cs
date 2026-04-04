using Ember.WebServer.Data;
using System.ComponentModel.DataAnnotations;

namespace Ember.WebServer.Areas.Knowledge.Entities;


/// <summary>
/// we can define tag and tag user, content, collection
/// </summary>
public class Tag
{
    public Guid Id { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public Guid? EmberUserId { get; set; }
    public EmberUser? EmberUser { get; set; }
    /// <summary>
    /// Gets or sets a value indicating whether the item is marked as private.
    /// Private tags are only visible to the user who created them and are not shared publicly.
    /// When Tag is Private all associated ItemTags should also be considered private, regardless of their individual IsPrivate settings. This ensures that the privacy setting of the Tag takes precedence and maintains consistent visibility rules for all related tags.
    /// </summary>
    public bool IsPrivate { get; set; }
    /// <summary>
    /// Gets or sets a value indicating whether a confidence rate is available for the current result.
    /// </summary>
    public bool HasConfidenceRate { get; set; }
    public string? Name { get; set; }
}

public class TagItem
{
    public Guid Id { get; set; }

    public Guid? ContentId { get; set; }
    public Content? Content { get; set; }

    public Guid? CollectionId { get; set; }
    public Collection? Collection { get; set; }

    public Guid EmberUserId { get; set; }
    public EmberUser EmberUser { get; set; } = null!; 
    public required Guid TagId { get; set; }
    /// <summary>
    /// Number between 0 and 100 indicating the confidence level of the tag's relevance to the content. This can be used for sorting or filtering tags based on their relevance.
    /// if tag has confidence rate, it means the system has calculated a confidence score for how well the tag applies to the content. This allows users to see which tags are most relevant and can help prioritize or filter tags based on their confidence levels.
    /// </summary>
    [Range(0, 100)]
    public byte? ConfidenceRate { get; set; } = 0;
    /// <summary>
    /// Gets or sets a value indicating whether the item is marked as private.
    /// Private tags are only visible to the user who created them and are not shared publicly. This allows users to have personal tags for their own organization and categorization without exposing them to others.
    /// </summary>
    public bool IsPrivate { get; set; }
    public Tag Tag { get; set; } = null!;
}
