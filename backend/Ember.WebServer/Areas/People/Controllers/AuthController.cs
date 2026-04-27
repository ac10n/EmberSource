using Ember.WebServer.Areas.People.Services;
using Ember.Domain.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Ember.Service.Models;
using Ember.Service;
using Microsoft.AspNetCore.WebUtilities;
using System.Text;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Extensions.Options;
using System.Text.Encodings.Web;
using System.Net.Http.Json;
using System.Text.Json;
using Ember.Domain.EmberEntities;

namespace Ember.WebServer.Areas.People.Controllers;

[ApiController]
[Route("api/v01/[controller]/[action]")]
public sealed class AuthController(
    SignInManager<EmberUser> signInManager,
    UserManager<EmberUser> userManager,
    TokenService tokenService,
    RoleManager<EmberRole> roleManager,
    IHttpClientFactory httpClientFactory,
    AuthSettings authSettings,
    IEmailSender emailSender,
    IWebHostEnvironment environment,
    IEmberDbContext dbContext) : ControllerBase
{
    [HttpPost]
    [AllowAnonymous]
    public async Task<ActionResult<TokenResponse>> Login(LoginRequest req)
    {
        var user = await userManager.FindByNameAsync(req.UserName);
        if (user is null)
        {
            return Unauthorized();
        }

        var result = await signInManager.CheckPasswordSignInAsync(user, req.Password, lockoutOnFailure: true);
        if (!result.Succeeded)
        {
            return Unauthorized();
        }

        var ip = HttpContext.Connection.RemoteIpAddress?.ToString();
        var deviceId = Request.Headers.UserAgent.ToString(); // or your own device id header

        var tokens = await tokenService.IssueTokensAsync(user, deviceId, ip);
        return tokens;
    }

    [HttpPost]
    [AllowAnonymous]
    public async Task<ActionResult<TokenResponse>> Refresh(RefreshRequest req)
    {
        var ip = HttpContext.Connection.RemoteIpAddress?.ToString();
        var tokens = await tokenService.RefreshAsync(req.RefreshToken, ip);
        if (tokens is null)
        {
            return Unauthorized();
        }
        return tokens;
    }

    [HttpPost]
    [Authorize]
    public async Task<IActionResult> Logout()
    {
        var userIdString = User.FindFirst("sub")?.Value;
        if (userIdString is null)
        {
            return Unauthorized();
        }

        var userId = Guid.Parse(userIdString);

        await tokenService.RevokeAllAsync(userId);
        return NoContent();
    }

    [HttpPost]
    [AllowAnonymous]
    public async Task<IActionResult> Register(RegisterDto dto)
    {
        var invitation = await dbContext.Invitations.FirstOrDefaultAsync(i => i.InviteCode == dto.InviteCode);
        if (invitation == null || invitation.AcceptedAt.HasValue || (invitation.ExpiresAt.HasValue && invitation.ExpiresAt < DateTimeOffset.UtcNow))
        {
            return BadRequest(new { error = "Invalid or expired invitation code" });
        }

        if (!invitation.IsInLegalAge)
        {
            return BadRequest(new { error = "You must be of legal age to register" });
        }

        var existing = await userManager.FindByEmailAsync(dto.Email);
        if (existing != null)
        {
            return BadRequest(new { error = "Email already in use" });
        }

        var user = new EmberUser
        {
            UserName = dto.UserName,
            Email = dto.Email,
            FullName = invitation.RealName,
            Jurisdiction = invitation.Jurisdiction,
            BirthYear = 1990 // placeholder, since IsInLegalAge is true
        };

        var result = await userManager.CreateAsync(user, dto.Password);
        if (!result.Succeeded)
        {
            return BadRequest(result.Errors);
        }

        // Add default role
        await userManager.AddToRoleAsync(user, KnownRoles.RegularMember.Name!);

        // Grant AllowToInviteUser claim — all members can invite
        await userManager.AddClaimAsync(user, new Claim(ClaimConstants.AllowToInviteUser, "true"));

        // Update invitation
        invitation.AcceptedAt = DateTimeOffset.UtcNow;
        invitation.AcceptedByUserId = user.Id;
        await dbContext.SaveChangesAsync();

        // Send confirmation email
        var emailToken = await userManager.GenerateEmailConfirmationTokenAsync(user);
        var callbackUrl = Url.Action("ConfirmEmail", "Auth", new { userId = user.Id, token = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(emailToken)) }, Request.Scheme);
        var html = $"<p>Welcome {user.FullName},</p><p>Please confirm your email by clicking <a href=\"{callbackUrl}\">here</a>.</p>";
        try
        {
            await emailSender.SendEmailAsync(user.Email!, "Confirm your email", html);
        }
        catch
        {
            // Log is handled by EmailSender; do not block registration on email failures
        }

        return Ok(new { message = "Registration successful. Please confirm your email." });
    }
    [HttpGet]
    [AllowAnonymous]
    public async Task<IActionResult> ConfirmEmail([FromQuery] Guid userId, [FromQuery] string token)
    {
        var user = await userManager.FindByIdAsync(userId.ToString());
        if (user == null)
        {
            return NotFound();
        }

        var decoded = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(token));
        var result = await userManager.ConfirmEmailAsync(user, decoded);
        if (!result.Succeeded)
        {
            return BadRequest(result.Errors);
        }

        var ip = HttpContext.Connection.RemoteIpAddress?.ToString();
        var deviceId = Request.Headers.UserAgent.ToString();
        var tokens = await tokenService.IssueTokensAsync(user, deviceId, ip);
        return Ok(new { message = "Email confirmed", token = tokens.AccessToken });
    }

    [HttpPost]
    [AllowAnonymous]
    public async Task<IActionResult> ForgotPassword(ForgotPasswordDto dto)
    {
        var user = await userManager.FindByEmailAsync(dto.Email);
        if (user == null)
        {
            return Ok(); // don't reveal user existence
        }

        var token = await userManager.GeneratePasswordResetTokenAsync(user);
        var callbackUrl = Url.Action("ResetPassword", "Auth", new { userId = user.Id, token = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(token)) }, Request.Scheme);
        var html = $"<p>Reset your password by clicking <a href=\"{callbackUrl}\">here</a>.</p>";
        try
        {
            await emailSender.SendEmailAsync(user.Email!, "Reset your password", html);
        }
        catch
        {
            // Log handled by EmailSender
        }

        return Ok();
    }
    [HttpPost]
    [AllowAnonymous]
    public async Task<IActionResult> ResetPassword(ResetPasswordDto dto)
    {
        var user = await userManager.FindByIdAsync(dto.UserId);
        if (user == null)
        {
            return NotFound();
        }

        var decoded = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(dto.Token));
        var result = await userManager.ResetPasswordAsync(user, decoded, dto.NewPassword);
        if (!result.Succeeded)
        {
            return BadRequest(result.Errors);
        }

        return Ok();
    }

    [HttpPost("external/{provider}")]
    [AllowAnonymous]
    public async Task<IActionResult> ExternalLogin([FromRoute] string provider, ExternalTokenDto dto)
    {
        if (string.IsNullOrEmpty(dto.AccessToken))
        {
            return BadRequest(new { error = "Missing access token" });
        }

        var (valid, email, providerUserId) = provider.ToLower() switch
        {
            "google" => await VerifyGoogleTokenAsync(dto.AccessToken),
            "facebook" => await VerifyFacebookTokenAsync(dto.AccessToken),
            _ => (false, null, null)
        };

        if (!valid || string.IsNullOrEmpty(email))
        {
            return Unauthorized(new { error = "Invalid external token or missing email" });
        }

        var user = await userManager.FindByEmailAsync(email);
        if (user == null)
        {
            user = new EmberUser
            {
                UserName = email,
                Email = email,
                FullName = email, // placeholder
                Jurisdiction = "Unknown",
                BirthYear = 1990
            };
            var createResult = await userManager.CreateAsync(user);
            if (!createResult.Succeeded)
            {
                return BadRequest(createResult.Errors);
            }

            await userManager.AddLoginAsync(user, new UserLoginInfo(provider, providerUserId ?? email, provider));
            await userManager.AddToRoleAsync(user, "Member");
        }

        var ip = HttpContext.Connection.RemoteIpAddress?.ToString();
        var deviceId = Request.Headers.UserAgent.ToString();
        var tokens = await tokenService.IssueTokensAsync(user, deviceId, ip);
        return Ok(tokens);
    }

    private async Task<(bool valid, string? email, string? providerUserId)> VerifyGoogleTokenAsync(string accessToken)
    {
        try
        {
            var client = httpClientFactory.CreateClient();
            var resp = await client.GetAsync($"https://www.googleapis.com/oauth2/v3/userinfo?access_token={Uri.EscapeDataString(accessToken)}");
            if (!resp.IsSuccessStatusCode)
                return (false, null, null);

            var json = await resp.Content.ReadAsStringAsync();
            using var doc = System.Text.Json.JsonDocument.Parse(json);
            var root = doc.RootElement;
            var email = root.GetProperty("email").GetString();
            var sub = root.GetProperty("sub").GetString();
            return (!string.IsNullOrEmpty(email), email, sub);
        }
        catch
        {
            return (false, null, null);
        }
    }

    private async Task<(bool valid, string? email, string? providerUserId)> VerifyFacebookTokenAsync(string accessToken)
    {
        try
        {
            var client = httpClientFactory.CreateClient();
            var appToken = $"{authSettings.Facebook.AppId}|{authSettings.Facebook.AppSecret}";
            var debugResp = await client.GetAsync($"https://graph.facebook.com/debug_token?input_token={Uri.EscapeDataString(accessToken)}&access_token={Uri.EscapeDataString(appToken)}");
            if (!debugResp.IsSuccessStatusCode)
                return (false, null, null);

            var debugJson = await debugResp.Content.ReadAsStringAsync();
            using var debugDoc = System.Text.Json.JsonDocument.Parse(debugJson);
            var data = debugDoc.RootElement.GetProperty("data");
            var isValid = data.GetProperty("is_valid").GetBoolean();
            if (!isValid)
                return (false, null, null);

            var userId = data.GetProperty("user_id").GetString();

            var resp = await client.GetAsync($"https://graph.facebook.com/{userId}?fields=id,email&access_token={Uri.EscapeDataString(accessToken)}");
            if (!resp.IsSuccessStatusCode)
                return (false, null, null);

            var json = await resp.Content.ReadAsStringAsync();
            using var doc = System.Text.Json.JsonDocument.Parse(json);
            var root = doc.RootElement;
            var email = root.TryGetProperty("email", out var em) ? em.GetString() : null;
            return (true, email, userId);
        }
        catch
        {
            return (false, null, null);
        }
    }
}
