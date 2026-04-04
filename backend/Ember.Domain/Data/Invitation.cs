using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Ember.Domain.Data;

/// <summary>
/// Represents an invitation sent by an existing Ember user to allow a new person to register.
/// The invited person must accept the invitation to create a new EmberUser account.
/// </summary>
public sealed class Invitation
{
    [Key]
    public Guid Id { get; set; }

    [Required]
    public required Guid InvitedByUserId { get; set; }

    [ForeignKey(nameof(InvitedByUserId))]
    public EmberUser? InvitedByUser { get; set; }

    // The real name provided when creating the invitation
    [Required]
    public required string RealName { get; set; }

    // True when the invited person is confirmed to be of legal age
    public bool IsInLegalAge { get; set; }

    // Jurisdiction provided for the invited person
    [Required]
    public required string Jurisdiction { get; set; }

    // Contact information for the invited person (email or phone or both)
    public string? Email { get; set; }
    public string? Phone { get; set; }

    // A unique code/token used to accept the invitation
    [Required]
    public required string InviteCode { get; set; }

    public required DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset? ExpiresAt { get; set; }

    // When set, indicates the invitation was accepted and a user was created
    public DateTimeOffset? AcceptedAt { get; set; }

    // If accepted, link to the created user
    public Guid? AcceptedByUserId { get; set; }

    [ForeignKey(nameof(AcceptedByUserId))]
    public EmberUser? AcceptedByUser { get; set; }
}
