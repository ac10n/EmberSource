using Microsoft.AspNetCore.Identity;

namespace Ember.WebServer.Data;

public class EmberUser : IdentityUser<Guid>
{
    public int BirthYear { get; set; }
    public required string FullName { get; set; }
    public required string Jurisdiction { get; set; }
    
}

public class EmberRole : IdentityRole<Guid>
{
    // Additional properties for roles can be added here if needed
}
