namespace Ember.WebServer.Areas.People.Models;

public class ProfileResponse
{
    public required string Username { get; set; }
    public required string FullName { get; set; }
    public required int BirthYear { get; set; }
    public required string Jurisdiction { get; set; }
}

public class ProfileRequest
{
    public string? ProfileId { get; set; }
}

public class UpdateProfileRequest
{
    public required string FullName { get; set; }
    public required int BirthYear { get; set; }
    public required string Jurisdiction { get; set; }
}

public class ChangePasswordRequest
{
    public string? OldPassword { get; set; }
    public string? NewPassword { get; set; }

}
