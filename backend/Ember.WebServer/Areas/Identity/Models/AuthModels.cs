namespace Ember.WebServer.Areas.Identity.Models;

public sealed record TokenResponse(
    string AccessToken,
    DateTimeOffset AccessTokenExpiresAt,
    string RefreshToken,
    DateTimeOffset RefreshTokenExpiresAt);

public sealed record LoginRequest(string UserName, string Password, bool RememberMe);
public sealed record RefreshRequest(string RefreshToken);
