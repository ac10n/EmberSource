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
public class ContentInteractionsController(IEmberDbContext dbContext) : ControllerBase
{
    [HttpGet]
    [Authorize]
    public async Task<ActionResult<IEnumerable<ContentInteraction>>> GetInteractions()
    {
        var userId = Guid.Parse(User.FindFirst("sub")?.Value ?? Guid.Empty.ToString());

        var interactions = await dbContext.ContentInteractions
            .Include(ci => ci.Content)
            .Where(ci => ci.UserId == userId)
            .ToListAsync();

        return Ok(interactions);
    }

    [HttpGet("{id}")]
    [Authorize]
    public async Task<ActionResult<ContentInteraction>> GetInteraction(Guid id)
    {
        var userId = Guid.Parse(User.FindFirst("sub")?.Value ?? Guid.Empty.ToString());

        var interaction = await dbContext.ContentInteractions
            .Include(ci => ci.Content)
            .FirstOrDefaultAsync(ci => ci.Id == id && ci.UserId == userId);

        if (interaction == null)
        {
            return NotFound();
        }

        return Ok(interaction);
    }

    [HttpGet("by-content/{contentId}")]
    [Authorize]
    public async Task<ActionResult<ContentInteraction>> GetInteractionByContent(Guid contentId)
    {
        var userId = Guid.Parse(User.FindFirst("sub")?.Value ?? Guid.Empty.ToString());

        var interaction = await dbContext.ContentInteractions
            .Include(ci => ci.Content)
            .FirstOrDefaultAsync(ci => ci.ContentId == contentId && ci.UserId == userId);

        if (interaction == null)
        {
            return NotFound();
        }

        return Ok(interaction);
    }

    [HttpPost]
    [Authorize]
    public async Task<ActionResult<ContentInteraction>> CreateInteraction(ContentInteraction interaction)
    {
        var userId = Guid.Parse(User.FindFirst("sub")?.Value ?? Guid.Empty.ToString());
        interaction.Id = Guid.NewGuid();
        interaction.UserId = userId;

        // Ensure no duplicate for same content
        var existing = await dbContext.ContentInteractions
            .FirstOrDefaultAsync(ci => ci.ContentId == interaction.ContentId && ci.UserId == userId);

        if (existing != null)
        {
            return Conflict("Interaction already exists for this content");
        }

        dbContext.ContentInteractions.Add(interaction);
        await dbContext.SaveChangesAsync();

        return CreatedAtAction(nameof(GetInteraction), new { id = interaction.Id }, interaction);
    }

    [HttpPut("{id}")]
    [Authorize]
    public async Task<IActionResult> UpdateInteraction(Guid id, ContentInteraction updatedInteraction)
    {
        var userId = Guid.Parse(User.FindFirst("sub")?.Value ?? Guid.Empty.ToString());

        var existing = await dbContext.ContentInteractions
            .FirstOrDefaultAsync(ci => ci.Id == id && ci.UserId == userId);

        if (existing == null)
        {
            return NotFound();
        }

        existing.IsRead = updatedInteraction.IsRead;
        existing.ReadAt = updatedInteraction.ReadAt;
        existing.IsLiked = updatedInteraction.IsLiked;
        existing.IsDisliked = updatedInteraction.IsDisliked;
        existing.Recommend = updatedInteraction.Recommend;
        existing.RemindLaterList = updatedInteraction.RemindLaterList;
        existing.Notes = updatedInteraction.Notes;

        await dbContext.SaveChangesAsync();

        return NoContent();
    }

    [HttpDelete("{id}")]
    [Authorize]
    public async Task<IActionResult> DeleteInteraction(Guid id)
    {
        var userId = Guid.Parse(User.FindFirst("sub")?.Value ?? Guid.Empty.ToString());

        var interaction = await dbContext.ContentInteractions
            .FirstOrDefaultAsync(ci => ci.Id == id && ci.UserId == userId);

        if (interaction == null)
        {
            return NotFound();
        }

        dbContext.ContentInteractions.Remove(interaction);
        await dbContext.SaveChangesAsync();

        return NoContent();
    }
}