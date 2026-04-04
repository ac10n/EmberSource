using System.ComponentModel.DataAnnotations;

namespace Ember.Domain.Areas.People.Data;

public sealed class RefreshToken
{
    [Key]
    public Guid Id { get; set; }

    [Required]
    public required Guid UserId { get; set; }

    [Required]
    public required string TokenHash { get; set; }

    public required DateTimeOffset ExpiresAt { get; set; }
    public required DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset? RevokedAt { get; set; }

    public Guid? ReplacedByTokenId { get; set; }

    public string? DeviceId { get; set; }
    public string? CreatedByIp { get; set; }
}


