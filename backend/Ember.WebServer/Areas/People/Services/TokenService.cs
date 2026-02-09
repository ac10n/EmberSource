using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Ember.WebServer.Areas.People.Config;
using Ember.WebServer.Areas.People.Data;
using Ember.WebServer.Areas.People.Models;
using Ember.WebServer.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace Ember.WebServer.Areas.People.Services;

public sealed class TokenService(
        EmberDbContext dbContext,
        UserManager<EmberUser> userManager,
        IOptions<JwtOptions> opt)
{
    public async Task<TokenResponse> IssueTokensAsync(EmberUser user, string? deviceId = null, string? ip = null)
    {
        var now = DateTimeOffset.UtcNow;

        var accessExpires = now.AddMinutes(opt.Value.AccessTokenMinutes);
        var accessToken = await CreateAccessTokenAsync(user, accessExpires);

        var refreshExpires = now.AddDays(opt.Value.RefreshTokenDays);
        var refreshRaw = CreateRefreshTokenRaw();
        var refreshHash = HashToken(refreshRaw);

        var rt = new RefreshToken
        {
            Id = Guid.NewGuid(),
            UserId = user.Id,
            TokenHash = refreshHash,
            CreatedAt = now,
            ExpiresAt = refreshExpires,
            DeviceId = deviceId,
            CreatedByIp = ip
        };

        dbContext.RefreshTokens.Add(rt);
        await dbContext.SaveChangesAsync();

        return new TokenResponse(accessToken, accessExpires, refreshRaw, refreshExpires);
    }

    public async Task<TokenResponse?> RefreshAsync(string refreshTokenRaw, string? ip = null)
    {
        var now = DateTimeOffset.UtcNow;
        var hash = HashToken(refreshTokenRaw);

        var existing = await dbContext.RefreshTokens
            .SingleOrDefaultAsync(x => x.TokenHash == hash);

        if (existing is null) return null;
        if (existing.RevokedAt is not null) return null;
        if (existing.ExpiresAt <= now) return null;

        // Rotation: revoke old token, issue a new one, link them.
        existing.RevokedAt = now;

        var user = await userManager.FindByIdAsync(existing.UserId.ToString());
        if (user is null)
        {
            return null;
        }

        var response = await IssueTokensAsync(user, deviceId: existing.DeviceId, ip: ip);

        // Link replaced-by (optional, but nice for auditing)
        var newHash = HashToken(response.RefreshToken);
        var newRow = await dbContext.RefreshTokens.SingleAsync(x => x.TokenHash == newHash);
        existing.ReplacedByTokenId = newRow.Id;

        await dbContext.SaveChangesAsync();
        return response;
    }

    public async Task RevokeAllAsync(Guid userId)
    {
        var now = DateTimeOffset.UtcNow;

        var tokens = await dbContext.RefreshTokens
            .Where(x => x.UserId == userId && x.RevokedAt == null && x.ExpiresAt > now)
            .ToListAsync();

        foreach (var t in tokens)
            t.RevokedAt = now;

        await dbContext.SaveChangesAsync();
    }

    private async Task<string> CreateAccessTokenAsync(EmberUser user, DateTimeOffset expiresAt)
    {
        var claims = new List<Claim>
        {
            new(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString("N")),
            new("name", user.UserName ?? user.Email ?? user.Id.ToString())
        };

        // Add roles as "role" claims (works with [Authorize(Roles="...")])
        var roles = await userManager.GetRolesAsync(user);
        claims.AddRange(roles.Select(r => new Claim("role", r)));

        // (Optional) add extra claims from Identity
        var extraClaims = await userManager.GetClaimsAsync(user);
        claims.AddRange(extraClaims);

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(opt.Value.SigningKey));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: opt.Value.Issuer,
            audience: opt.Value.Audience,
            claims: claims,
            notBefore: DateTime.UtcNow,
            expires: expiresAt.UtcDateTime,
            signingCredentials: creds);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    private static string CreateRefreshTokenRaw()
    {
        // 256-bit random token, URL-safe base64
        var bytes = RandomNumberGenerator.GetBytes(32);
        return Base64UrlEncoder.Encode(bytes);
    }

    private static string HashToken(string tokenRaw)
    {
        // Store hash to reduce impact of DB leaks
        var bytes = SHA256.HashData(Encoding.UTF8.GetBytes(tokenRaw));
        return Convert.ToHexString(bytes); // uppercase hex
    }
}
