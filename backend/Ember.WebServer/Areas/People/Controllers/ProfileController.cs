using Ember.WebServer.Areas.People.Models;
using Ember.Domain.Data;
using Ember.WebServer.Helpers;
using Ember.WebServer.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;

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
        var userId = User.GetUserId()?.ToString();
        if (userId is null)
        {
            return Unauthorized(); // TODO: If a profile's visibility allows anonymous access, we should change this
        }
        var user = await userManager.FindByIdAsync(request.ProfileId ?? userId);
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
    public async Task<ActionResult<UpdateResult<ProfileResponse>>> UpdateMyProfile(UpdateProfileRequest request)
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

    private async Task<(bool flowControl, ActionResult<UpdateResult<ProfileResponse>> value)> NewMethod(ChangePasswordRequest request, EmberUser user)
    {
        if (!string.IsNullOrEmpty(request.NewPassword))
        {
            if (string.IsNullOrEmpty(request.OldPassword))
            {
                return (flowControl: false, value: BadRequest("Current password is required to set a new password."));
            }
            var passwordResult = await userManager.ChangePasswordAsync(user, request.OldPassword, request.NewPassword);
            if (!passwordResult.Succeeded)
            {
                return (flowControl: false, value: BadRequest(passwordResult.Errors));
            }
        }

        return (flowControl: true, value: null);
    }
}
