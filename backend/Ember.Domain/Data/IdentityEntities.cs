using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace Ember.Domain.Data;

public class EmberUser : IdentityUser<Guid>
{
    public int BirthYear { get; set; }
    public required string FullName { get; set; }
    public required string Jurisdiction { get; set; }

    [InverseProperty(nameof(EmberUserRole.User))]
    public ICollection<EmberUserRole>? UserRoles { get; set; }
}

public class EmberRole : IdentityRole<Guid>
{
    [InverseProperty(nameof(EmberUserRole.Role))]
    public ICollection<EmberUserRole>? UserRoles { get; set; }
}

public class EmberUserRole : IdentityUserRole<Guid>
{
    public required Guid Id { get; set; }

    [ForeignKey(nameof(UserId))]
    public EmberUser? User { get; set; }

    [ForeignKey(nameof(RoleId))]
    public EmberRole? Role { get; set; }

    public Guid? PlatformSectionId { get; set; }
    public PlatformSection? PlatformSection { get; set; }
}

public class EmberUserLogin: IdentityUserLogin<Guid> 
{

}

public class EmberRoleClaim: IdentityRoleClaim<Guid> 
{

}

public class EmberUserToken: IdentityUserToken<Guid> 
{

}
