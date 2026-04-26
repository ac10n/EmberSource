using Ember.Domain.Data;
using Ember.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Ember.WebServer.Areas.People.Controllers;

[ApiController]
[Route("api/v01/[controller]")]
[Authorize] // Require authentication, assume admin role check
public class BadgeDefinitionsController(IEmberDbContext dbContext) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IEnumerable<BadgeDefinition>>> GetBadgeDefinitions()
    {
        var badgeDefinitions = await dbContext.BadgeDefinitions.ToListAsync();
        return Ok(badgeDefinitions);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<BadgeDefinition>> GetBadgeDefinition(Guid id)
    {
        var badgeDefinition = await dbContext.BadgeDefinitions.FindAsync(id);
        if (badgeDefinition == null)
        {
            return NotFound();
        }

        return Ok(badgeDefinition);
    }

    [HttpPost]
    public async Task<ActionResult<BadgeDefinition>> CreateBadgeDefinition(BadgeDefinition badgeDefinition)
    {
        badgeDefinition.Id = Guid.NewGuid();

        dbContext.BadgeDefinitions.Add(badgeDefinition);
        await dbContext.SaveChangesAsync();

        return CreatedAtAction(nameof(GetBadgeDefinition), new { id = badgeDefinition.Id }, badgeDefinition);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateBadgeDefinition(Guid id, BadgeDefinition updatedBadgeDefinition)
    {
        var existing = await dbContext.BadgeDefinitions.FindAsync(id);
        if (existing == null)
        {
            return NotFound();
        }

        existing.Name = updatedBadgeDefinition.Name;
        existing.IsNumeric = updatedBadgeDefinition.IsNumeric;
        existing.IsFractional = updatedBadgeDefinition.IsFractional;
        existing.MinValue = updatedBadgeDefinition.MinValue;
        existing.MaxValue = updatedBadgeDefinition.MaxValue;
        existing.DefaultValue = updatedBadgeDefinition.DefaultValue;

        await dbContext.SaveChangesAsync();

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteBadgeDefinition(Guid id)
    {
        var badgeDefinition = await dbContext.BadgeDefinitions.FindAsync(id);
        if (badgeDefinition == null)
        {
            return NotFound();
        }

        dbContext.BadgeDefinitions.Remove(badgeDefinition);
        await dbContext.SaveChangesAsync();

        return NoContent();
    }
}