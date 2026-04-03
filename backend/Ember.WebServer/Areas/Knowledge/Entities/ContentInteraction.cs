using Ember.WebServer.Data;

namespace Ember.WebServer.Areas.Knowledge.Entities;

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
