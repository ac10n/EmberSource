using Ember.Domain.Data;
using Ember.Domain.EmberEntities;
using Ember.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Ember.WebServer.Areas.People.Controllers;

[ApiController]
[Route("api/v01/[controller]")]
[Authorize]
public class TagsController(IEmberDbContext dbContext) : ControllerBase
{
    [HttpGet]
    [Authorize]
    public async Task<ActionResult<IEnumerable<Tag>>> GetTags()
    {
        var userId = Guid.Parse(User.FindFirst("sub")?.Value ?? Guid.Empty.ToString());

        var tags = await dbContext.Tags
            .Include(t => t.EmberUser)
            .Where(t => !t.IsPrivate || t.EmberUserId == userId)
            .ToListAsync();

        return Ok(tags);
    }

    [HttpGet("{id}")]
    [Authorize]
    public async Task<ActionResult<Tag>> GetTag(Guid id)
    {
        var userId = Guid.Parse(User.FindFirst("sub")?.Value ?? Guid.Empty.ToString());

        var tag = await dbContext.Tags
            .Include(t => t.EmberUser)
            .FirstOrDefaultAsync(t => t.Id == id && (!t.IsPrivate || t.EmberUserId == userId));

        if (tag == null)
        {
            return NotFound();
        }

        return Ok(tag);
    }

    [HttpPost]
    [Authorize]
    public async Task<ActionResult<Tag>> CreateTag(Tag tag)
    {
        tag.Id = Guid.NewGuid();
        tag.EmberUserId = Guid.Parse(User.FindFirst("sub")?.Value ?? Guid.Empty.ToString());

        dbContext.Tags.Add(tag);
        await dbContext.SaveChangesAsync();

        return CreatedAtAction(nameof(GetTag), new { id = tag.Id }, tag);
    }

    [HttpPut("{id}")]
    [Authorize]
    public async Task<IActionResult> UpdateTag(Guid id, Tag updatedTag)
    {
        var existing = await dbContext.Tags.FindAsync(id);
        if (existing == null)
        {
            return NotFound();
        }

        // Check ownership
        var userId = Guid.Parse(User.FindFirst("sub")?.Value ?? Guid.Empty.ToString());
        if (existing.EmberUserId != userId)
        {
            return Forbid();
        }

        existing.Name = updatedTag.Name;
        existing.IsPrivate = updatedTag.IsPrivate;
        existing.HasConfidenceRate = updatedTag.HasConfidenceRate;

        await dbContext.SaveChangesAsync();

        return NoContent();
    }

    [HttpDelete("{id}")]
    [Authorize]
    public async Task<IActionResult> DeleteTag(Guid id)
    {
        var tag = await dbContext.Tags.FindAsync(id);
        if (tag == null)
        {
            return NotFound();
        }

        // Check ownership
        var userId = Guid.Parse(User.FindFirst("sub")?.Value ?? Guid.Empty.ToString());
        if (tag.EmberUserId != userId)
        {
            return Forbid();
        }

        dbContext.Tags.Remove(tag);
        await dbContext.SaveChangesAsync();

        return NoContent();
    }
}