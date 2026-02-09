using System.Security.Claims;

namespace Ember.WebServer.Helpers;

public static class AuthExtensions
{
    public static Guid? GetUserId(this ClaimsPrincipal user)
    {
        var claim = user.FindFirst("sub") ?? user.FindFirst(ClaimTypes.NameIdentifier);
        if (claim is null)
        {
            return null;
        }
        if (Guid.TryParse(claim.Value, out var userId))
        {
            return userId;
        }
        return null;
    }
}
