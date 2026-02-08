using Ember.WebServer.Areas.Identity.Models;
using Ember.WebServer.Areas.Identity.Services;
using Ember.WebServer.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Ember.WebServer.Areas.Identity.Controllers;

[ApiController]
[Route("auth")]
public sealed class AuthController(
    SignInManager<EmberUser> signInManager,
    UserManager<EmberUser> userManager,
    TokenService tokenService) : ControllerBase
{
    [HttpPost("login")]
    [AllowAnonymous]
    public async Task<ActionResult<TokenResponse>> Login([FromBody] LoginRequest req)
    {
        var user = await userManager.FindByNameAsync(req.UserName);
        if (user is null) return Unauthorized();

        var result = await signInManager.CheckPasswordSignInAsync(user, req.Password, lockoutOnFailure: true);
        if (!result.Succeeded) return Unauthorized();

        var ip = HttpContext.Connection.RemoteIpAddress?.ToString();
        var deviceId = Request.Headers.UserAgent.ToString(); // or your own device id header

        var tokens = await tokenService.IssueTokensAsync(user, deviceId, ip);
        return Ok(tokens);
    }

    [HttpPost("refresh")]
    [AllowAnonymous]
    public async Task<ActionResult<TokenResponse>> Refresh([FromBody] RefreshRequest req)
    {
        var ip = HttpContext.Connection.RemoteIpAddress?.ToString();
        var tokens = await tokenService.RefreshAsync(req.RefreshToken, ip);
        if (tokens is null) return Unauthorized();
        return Ok(tokens);
    }

    [HttpPost("logout")]
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
}
