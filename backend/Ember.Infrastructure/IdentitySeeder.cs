using Ember.Domain.EmberEntities;
using Ember.Infrastructure;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Ember.Domain.Data;

public static class IdentitySeeder
{
    public static async Task SeedAsync(IServiceProvider services)
    {
        var roleManager = services.GetRequiredService<RoleManager<EmberRole>>();
        var userManager = services.GetRequiredService<UserManager<EmberUser>>();
        
        await SeedRoles(roleManager);
        await SeedUsers(services, userManager);
        await SeedUserRoles(roleManager, userManager);

        await SeedDataOwnershipTypes(services);
        await SeedFinancialModels(services);
        await SeedPlatformSections(services);
        await SeedContentTypes(services);
        await SeedContentFormats(services);
        await SeedContentVisibilities(services);
        await SeedInitialContents(services);
    }

    private static async Task SeedUserRoles(RoleManager<EmberRole> roleManager, UserManager<EmberUser> userManager)
    {
        foreach (var userRole in KnownUserRoles.AllKnownUserRoles())
        {
            var user = await userManager.FindByIdAsync(userRole.UserId.ToString());
            if (user == null)
            {
                continue;
            }

            var role = await roleManager.FindByIdAsync(userRole.RoleId.ToString());
            if (role == null)
            {
                continue;
            }

            if (await userManager.IsInRoleAsync(user, role.Name))
            {
                continue;
            }
            await userManager.AddToRoleAsync(user, role.Name);
        }
    }

    private static async Task SeedUsers(IServiceProvider services, UserManager<EmberUser> userManager)
    {
        foreach (var user in KnownUsers.AllKnownUsers())
        {
            if (await userManager.FindByIdAsync(user.Id.ToString()) != null)
            {
                continue;
            }
            await userManager.CreateAsync(user);
            if (user.UserName == "ali")
            {
                var password = services.GetRequiredService<IConfiguration>()["Secrets:FounderPassword"]!;
                var result = await userManager.AddPasswordAsync(user, password);
                if (!result.Succeeded)
                {
                    throw new Exception($"Failed to set password for user {user.UserName}: {string.Join(", ", result.Errors.Select(e => e.Description))}");
                }
            }
        }
    }

    private static async Task SeedRoles(RoleManager<EmberRole> roleManager)
    {
        if (!await roleManager.Roles.AnyAsync())
        {
            foreach (var role in KnownRoles.AllKnownRoles())
            {
                await roleManager.CreateAsync(role);
            }
        }
    }

    private static async Task SeedInitialContents(IServiceProvider services)
    {
        var dbContext = services.GetRequiredService<EmberDbContext>();
        if (await dbContext.Contents.AnyAsync())
        {
            return;
        }
        await dbContext.Contents.AddRangeAsync(KnownContent.AllKnownContents());
        await dbContext.SaveChangesAsync();
    }

    private static async Task SeedContentVisibilities(IServiceProvider services)
    {
        var dbContext = services.GetRequiredService<EmberDbContext>();
        if (await dbContext.ContentVisibilities.AnyAsync())
        {
            return;
        }
        await dbContext.ContentVisibilities.AddRangeAsync(Enum.GetValues<ContentVisibilityEnum>()
                .Select(cv => new ContentVisibility
                {
                    Id = cv,
                    Name = cv.ToString()
                })
            );
        await dbContext.SaveChangesAsync();
    }

    private static async Task SeedContentFormats(IServiceProvider services)
    {
        var dbContext = services.GetRequiredService<EmberDbContext>();
        if (await dbContext.ContentFormats.AnyAsync())
        {
            return;
        }
        await dbContext.ContentFormats.AddRangeAsync(Enum.GetValues<ContentFormatEnum>()
            .Select(cf => new ContentFormat
            {
                Id = cf,
                Name = cf.ToString()
            })
        );
        await dbContext.SaveChangesAsync();
    }

    private static async Task SeedContentTypes(IServiceProvider services)
    {
        var dbContext = services.GetRequiredService<EmberDbContext>();
        if (await dbContext.ContentTypes.AnyAsync())
        {
            return;
        }
        await dbContext.ContentTypes.AddRangeAsync(Enum.GetValues<ContentTypes>()
            .Select(ct => new ContentType
            {
                Id = ct,
                Name = ct.ToString()
            })
        );
        await dbContext.SaveChangesAsync();
    }

    private static async Task SeedPlatformSections(IServiceProvider services)
    {
        var dbContext = services.GetRequiredService<EmberDbContext>();
        if (await dbContext.PlatformSections.AnyAsync())
        {
            return;
        }
        await dbContext.PlatformSections.AddRangeAsync(KnownPlatformSections.AllKnownPlatformSections());
        await dbContext.SaveChangesAsync();
    }

    private static async Task SeedFinancialModels(IServiceProvider services)
    {
        var dbContext = services.GetRequiredService<EmberDbContext>();
        if (await dbContext.FinancialModels.AnyAsync())
        {
            return;
        }
        await dbContext.FinancialModels.AddRangeAsync(KnownFinancialModels.AllModels);
        await dbContext.SaveChangesAsync();
    }

    private static async Task SeedDataOwnershipTypes(IServiceProvider services)
    {
        var dbContext = services.GetRequiredService<EmberDbContext>();
        if (await dbContext.DataOwnerships.AnyAsync())
        {
            return;
        }
        await dbContext.DataOwnerships.AddRangeAsync(Enum.GetValues<DataOwnershipType>()
                .Select(ownershipType => new DataOwnership
                {
                    Id = ownershipType,
                    Name = ownershipType.ToString()
                })
            );
        await dbContext.SaveChangesAsync();
    }
}
