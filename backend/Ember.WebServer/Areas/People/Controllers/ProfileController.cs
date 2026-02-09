using Ember.WebServer.Areas.People.Models;
using Ember.WebServer.Data;
using Ember.WebServer.Helpers;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;

namespace Ember.WebServer.Areas.People.Controllers;

[ApiController]
[Authorize]
[Route("api/v01/[controller]/[action]")]
public sealed class ProfileController(
    UserManager<EmberUser> userManager,
    EmberDbContext dbContext
    ) : ControllerBase
{
    [HttpPost]
    public async Task<ActionResult<ProfileResponse>> GetMyProfile()
    {
        var userId = User.GetUserId();
        if (userId is null)
        {
            return Unauthorized();
        }
        var user = await userManager.FindByIdAsync(userId.ToString());
        if (user is null)
        {
            return Unauthorized();
        }

        var profile = new ProfileResponse
        {
            Username = user.UserName,
            FullName = user.FullName,
            BirthYear = user.BirthYear,
            Jurisdiction = user.Jurisdiction
        };

        return profile;
    }

    [HttpPost]
    public async Task<IActionResult> UpdateMyProfile(UpdateProfileRequest req)
    {
        var userId = User.GetUserId();
        if (userId is null)
        {
            return Unauthorized();
        }

        var user = await userManager.FindByIdAsync(userId.ToString());
        if (user is null)
        {
            return Unauthorized();
        }

        user.FullName = req.FullName;
        user.BirthYear = req.BirthYear;
        user.Jurisdiction = req.Jurisdiction;

        var result = await userManager.UpdateAsync(user);
        if (!result.Succeeded)
        {
            return BadRequest(result.Errors);
        }

        if (!string.IsNullOrEmpty(req.NewPassword))
        {
            if (string.IsNullOrEmpty(req.OldPassword))
            {
                return BadRequest("Current password is required to set a new password.");
            }
            var passwordResult = await userManager.ChangePasswordAsync(user, req.OldPassword, req.NewPassword);
            if (!passwordResult.Succeeded)
            {
                return BadRequest(passwordResult.Errors);
            }
        }

        return NoContent();
    }
}
