namespace Ember.WebServer.Areas.People.Models;

public class ProfileResponseModel
{
    public required string Username { get; set; }
    public required string FullName { get; set; }
    public required int BirthYear { get; set; }
    public required string Jurisdiction { get; set; }
}

public class UpdateProfileRequestModel
{
    public required string FullName { get; set; }
    public required int BirthYear { get; set; }
    public required string Jurisdiction { get; set; }
    public string? NewPassword { get; set; }
}
