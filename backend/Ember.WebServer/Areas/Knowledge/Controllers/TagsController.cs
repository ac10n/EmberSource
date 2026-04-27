using Ember.WebServer.Areas.Knowledge.Models;
using Ember.WebServer.Areas.Knowledge.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Ember.WebServer.Areas.Knowledge.Controllers;

[ApiController]
[Route("api/v01/[controller]")]
[Authorize]
public class TagsController(IKnowledgeService knowledgeService) : ControllerBase
{
    [HttpGet]
    [Authorize]
    public async Task<ActionResult<IEnumerable<TagModel>>> GetTags()
    {
        var tags = await knowledgeService.GetTags();
        return Ok(tags);
    }

    [HttpGet("{id}")]
    [Authorize]
    public async Task<ActionResult<TagModel>> GetTag(Guid id)
    {
        var tag = await knowledgeService.GetTag(id);

        if (tag == null)
        {
            return NotFound();
        }

        return Ok(tag);
    }

    [HttpPost]
    [Authorize]
    public async Task<ActionResult<TagModel>> CreateTag(TagCreateModel createModel)
    {
        try
        {
            var tag = await knowledgeService.CreateTag(createModel);
            return CreatedAtAction(nameof(GetTag), new { id = tag.Id }, tag);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPut("{id}")]
    [Authorize]
    public async Task<IActionResult> UpdateTag(Guid id, TagUpdateModel updateModel)
    {
        try
        {
            var tag = await knowledgeService.UpdateTag(id, updateModel);
            return NoContent();
        }
        catch (KeyNotFoundException)
        {
            return NotFound();
        }
        catch (UnauthorizedAccessException)
        {
            return Forbid();
        }
    }

    [HttpDelete("{id}")]
    [Authorize]
    public async Task<IActionResult> DeleteTag(Guid id)
    {
        try
        {
            await knowledgeService.DeleteTag(id);
            return NoContent();
        }
        catch (KeyNotFoundException)
        {
            return NotFound();
        }
        catch (UnauthorizedAccessException)
        {
            return Forbid();
        }
    }
}