namespace Ember.Service;

public static class ClaimConstants
{
    /// <summary>Claim type that grants invite creation ability.</summary>
    public const string AllowToInviteUser = nameof(AllowToInviteUser);

    /// <summary>Claim type that grants direct user registration ability (Admin only).</summary>
    public const string AllowToRegisterUser = nameof(AllowToRegisterUser);
}
