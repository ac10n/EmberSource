namespace Ember.WebServer.Helpers;

/// <summary>
/// Authorization policy names. Claim types are defined in <see cref="Ember.Service.ClaimConstants"/>.
/// </summary>
public static class PolicyConstants
{
    /// <summary>Policy requiring the <see cref="Ember.Service.ClaimConstants.AllowToInviteUser"/> claim.</summary>
    public const string AllowToInviteUser = nameof(AllowToInviteUser);

    /// <summary>Policy requiring the <see cref="Ember.Service.ClaimConstants.AllowToRegisterUser"/> claim.</summary>
    public const string AllowToRegisterUser = nameof(AllowToRegisterUser);
}
