using Ember.Domain.Data;
using Ember.WebServer.Helpers;
using Ember.WebServer.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Ember.Service.Models;

namespace Ember.WebServer.Areas.People.Controllers;

[ApiController]
[Authorize]
[Route("api/v01/[controller]/[action]")]
public sealed class ProfileController(
    UserManager<EmberUser> userManager
    ) : ControllerBase
{
    [HttpPost]
    public async Task<ActionResult<ProfileResponse>> GetProfile(ProfileRequest request)
    {
        var userId = User.GetUserId();
        if (userId is null)
        {
            return Unauthorized(); // TODO: If a profile's visibility allows anonymous access, we should change this
        }
        var lookupId = request.ProfileId ?? userId.Value.ToString();
        var user = await userManager.FindByIdAsync(lookupId);
        if (user is null)
        {
            return Unauthorized();
        }

        var profile = new ProfileResponse
        {
            Username = user.UserName ?? string.Empty,
            FullName = user.FullName ?? string.Empty,
            BirthYear = user.BirthYear,
            Jurisdiction = user.Jurisdiction ?? string.Empty
        };

        return profile;
    }

    [HttpPost]
    public async Task<ActionResult<UpdateResult<ProfileResponse>>> UpdateMyProfile(UpdateProfileRequest request)
    {
        var userId = User.GetUserId();
        if (userId is null)
        {
            return Unauthorized();
        }

        var user = await userManager.FindByIdAsync(userId.Value.ToString());
        if (user is null)
        {
            return Unauthorized();
        }

        user.FullName = request.FullName;
        user.BirthYear = request.BirthYear;
        user.Jurisdiction = request.Jurisdiction;

        var result = await userManager.UpdateAsync(user);
        if (!result.Succeeded)
        {
            return BadRequest(result.Errors);
        }

        return new UpdateResult<ProfileResponse>
        {
            Result = UpdateResultKind.Success,
        };
    }

    [HttpPost]
    public async Task<ActionResult<UpdateResult<ProfileResponse>>> ChangePassword(ChangePasswordRequest request)
    {
        var userId = User.GetUserId();
        if (userId is null)
        {
            return Unauthorized();
        }

        var user = await userManager.FindByIdAsync(userId.Value.ToString());
        if (user is null)
        {
            return Unauthorized();
        }

        if (string.IsNullOrEmpty(request.NewPassword))
        {
            return BadRequest("New password is required.");
        }

        if (string.IsNullOrEmpty(request.OldPassword))
        {
            return BadRequest("Current password is required to set a new password.");
        }

        var passwordResult = await userManager.ChangePasswordAsync(user, request.OldPassword, request.NewPassword);
        if (!passwordResult.Succeeded)
        {
            return BadRequest(passwordResult.Errors);
        }

        return new UpdateResult<ProfileResponse>
        {
            Result = UpdateResultKind.Success,
        };
    }
}
