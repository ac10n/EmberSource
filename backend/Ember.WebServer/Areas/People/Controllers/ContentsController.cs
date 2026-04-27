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
public class ContentsController(IEmberDbContext dbContext) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Content>>> GetContents(
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 10,
        [FromQuery] Guid? userId = null)
    {
        var query = dbContext.Contents
            .Include(c => c.ContentType)
            .Include(c => c.ContentFormat)
            .Include(c => c.ContentVisibility)
            .Include(c => c.EmberUser)
            .Where(c => c.IsActive);

        if (userId.HasValue)
        {
            query = query.Where(c => c.EmberUserId == userId.Value);
        }

        var contents = await query
            .OrderByDescending(c => c.CreatedAt)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return Ok(contents);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Content>> GetContent(Guid id)
    {
        var content = await dbContext.Contents
            .Include(c => c.ContentType)
            .Include(c => c.ContentFormat)
            .Include(c => c.ContentVisibility)
            .Include(c => c.EmberUser)
            .Include(c => c.ParentContent)
            .Include(c => c.ChildContents)
            .FirstOrDefaultAsync(c => c.Id == id && c.IsActive);

        if (content == null)
        {
            return NotFound();
        }

        return Ok(content);
    }

    [HttpGet("{id}/versions")]
    public async Task<ActionResult<IEnumerable<Content>>> GetContentVersions(Guid id)
    {
        var identifier = await dbContext.Contents
            .Where(c => c.Id == id)
            .Select(c => c.Identifier)
            .FirstOrDefaultAsync();

        if (identifier == Guid.Empty)
        {
            return NotFound();
        }

        var versions = await dbContext.Contents
            .Where(c => c.Identifier == identifier)
            .OrderByDescending(c => c.Version)
            .ToListAsync();

        return Ok(versions);
    }

    [HttpPost]
    [Authorize]
    public async Task<ActionResult<Content>> CreateContent(Content content)
    {
        content.Id = Guid.NewGuid();
        content.Identifier = content.Id; // First version
        content.Version = 1;
        content.CreatedAt = DateTimeOffset.UtcNow;
        content.IsActive = true;
        content.EmberUserId = Guid.Parse(User.FindFirst("sub")?.Value ?? Guid.Empty.ToString());

        dbContext.Contents.Add(content);
        await dbContext.SaveChangesAsync();

        return CreatedAtAction(nameof(GetContent), new { id = content.Id }, content);
    }

    [HttpPut("{id}")]
    [Authorize]
    public async Task<IActionResult> UpdateContent(Guid id, Content updatedContent)
    {
        var existing = await dbContext.Contents.FindAsync(id);
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

        // Soft delete current version
        existing.IsActive = false;
        existing.RemovedTime = DateTimeOffset.UtcNow;

        // Create new version
        var newVersion = new Content
        {
            Id = Guid.NewGuid(),
            Identifier = existing.Identifier,
            Version = existing.Version + 1,
            ParentContentId = existing.Id,
            ContentTypeId = updatedContent.ContentTypeId,
            ContentFormatId = updatedContent.ContentFormatId,
            FormatVersion = updatedContent.FormatVersion,
            Title = updatedContent.Title,
            Data = updatedContent.Data,
            EmberUserId = userId,
            CreatedAt = DateTimeOffset.UtcNow,
            IsActive = true,
            ContentVisibilityId = updatedContent.ContentVisibilityId,
            VisibilityCriteria = updatedContent.VisibilityCriteria
        };

        dbContext.Contents.Add(newVersion);
        await dbContext.SaveChangesAsync();

        return NoContent();
    }

    [HttpDelete("{id}")]
    [Authorize]
    public async Task<IActionResult> DeleteContent(Guid id)
    {
        var content = await dbContext.Contents.FindAsync(id);
        if (content == null)
        {
            return NotFound();
        }

        // Check ownership
        var userId = Guid.Parse(User.FindFirst("sub")?.Value ?? Guid.Empty.ToString());
        if (content.EmberUserId != userId)
        {
            return Forbid();
        }

        content.IsActive = false;
        content.RemovedTime = DateTimeOffset.UtcNow;
        await dbContext.SaveChangesAsync();

        return NoContent();
    }
}