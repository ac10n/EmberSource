using Ember.Domain.Data;
using Ember.Domain.EmberEntities;
using Ember.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Ember.WebServer.Areas.Knowledge.Controllers;

[ApiController]
[Route("api/v01/[controller]")]
[Authorize]
public class RelatedContentsController(IEmberDbContext dbContext) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IEnumerable<RelatedContent>>> GetRelatedContents([FromQuery] Guid? contentId = null)
    {
        var query = dbContext.RelatedContents
            .Include(rc => rc.Content)
            .Include(rc => rc.RelatedContentItem)
            .Include(rc => rc.RelatedContentType)
            .AsQueryable();

        if (contentId.HasValue)
        {
            query = query.Where(rc => rc.ContentId == contentId.Value);
        }

        var relatedContents = await query.ToListAsync();
        return Ok(relatedContents);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<RelatedContent>> GetRelatedContent(Guid id)
    {
        var relatedContent = await dbContext.RelatedContents
            .Include(rc => rc.Content)
            .Include(rc => rc.RelatedContentItem)
            .Include(rc => rc.RelatedContentType)
            .FirstOrDefaultAsync(rc => rc.Id == id);

        if (relatedContent == null)
        {
            return NotFound();
        }

        return Ok(relatedContent);
    }

    [HttpPost]
    [Authorize]
    public async Task<ActionResult<RelatedContent>> CreateRelatedContent(RelatedContent relatedContent)
    {
        // Check if user owns the content
        var userId = Guid.Parse(User.FindFirst("sub")?.Value ?? Guid.Empty.ToString());
        var content = await dbContext.Contents.FindAsync(relatedContent.ContentId);
        if (content == null || content.EmberUserId != userId)
        {
            return Forbid();
        }

        relatedContent.Id = Guid.NewGuid();

        dbContext.RelatedContents.Add(relatedContent);
        await dbContext.SaveChangesAsync();

        return CreatedAtAction(nameof(GetRelatedContent), new { id = relatedContent.Id }, relatedContent);
    }

    [HttpPut("{id}")]
    [Authorize]
    public async Task<IActionResult> UpdateRelatedContent(Guid id, RelatedContent updatedRelatedContent)
    {
        var existing = await dbContext.RelatedContents.FindAsync(id);
        if (existing == null)
        {
            return NotFound();
        }

        // Check ownership
        var userId = Guid.Parse(User.FindFirst("sub")?.Value ?? Guid.Empty.ToString());
        var content = await dbContext.Contents.FindAsync(existing.ContentId);
        if (content == null || content.EmberUserId != userId)
        {
            return Forbid();
        }

        existing.RelatedContentId = updatedRelatedContent.RelatedContentId;
        existing.RelatedContentTypeId = updatedRelatedContent.RelatedContentTypeId;

        await dbContext.SaveChangesAsync();

        return NoContent();
    }

    [HttpDelete("{id}")]
    [Authorize]
    public async Task<IActionResult> DeleteRelatedContent(Guid id)
    {
        var relatedContent = await dbContext.RelatedContents.FindAsync(id);
        if (relatedContent == null)
        {
            return NotFound();
        }

        // Check ownership
        var userId = Guid.Parse(User.FindFirst("sub")?.Value ?? Guid.Empty.ToString());
        var content = await dbContext.Contents.FindAsync(relatedContent.ContentId);
        if (content == null || content.EmberUserId != userId)
        {
            return Forbid();
        }

        dbContext.RelatedContents.Remove(relatedContent);
        await dbContext.SaveChangesAsync();

        return NoContent();
    }
}