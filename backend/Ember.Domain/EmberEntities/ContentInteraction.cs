using Ember.Domain.Data;

namespace Ember.Domain.EmberEntities;

/// <summary>
/// Private interaction data between users and content, such as read status, likes/dislikes, notes, etc. This allows us to track user engagement and preferences without exposing this information publicly.
/// </summary>
public class ContentInteraction
{
    public Guid Id { get; set; }

    public required Guid ContentId { get; set; }
    public Content? Content { get; set; }

    public Guid? UserId { get; set; }
    public EmberUser? User { get; set; }

    public bool IsRead { get; set; }
    public DateTimeOffset? ReadAt { get; set; }

    public bool IsLiked { get; set; }
    public bool IsDisliked { get; set; }
    public bool Recommend { get; set; }
    public bool RemindLaterList { get; set; }
    public string? Notes { get; set; }
}
