namespace Ember.Service.Models;

public sealed record TokenResponse(
    string AccessToken,
    DateTimeOffset AccessTokenExpiresAt,
    string RefreshToken,
    DateTimeOffset RefreshTokenExpiresAt);

public sealed record LoginRequest(string UserName, string Password, bool RememberMe);
public sealed record RefreshRequest(string RefreshToken);

public sealed record RegisterDto(string InviteCode, string UserName, string Email, string Password);
public sealed record ForgotPasswordDto(string Email);
public sealed record ResetPasswordDto(string UserId, string Token, string NewPassword);
public sealed record LoginDto(string Email, string Password);
public sealed record ExternalTokenDto(string AccessToken, string? ProfileImageUrl);
