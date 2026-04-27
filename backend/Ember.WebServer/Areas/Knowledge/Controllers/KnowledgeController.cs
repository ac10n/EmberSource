using Ember.WebServer.Areas.Knowledge.Models;
using Ember.WebServer.Areas.Knowledge.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Ember.WebServer.Helpers;

namespace Ember.WebServer.Areas.Knowledge.Controllers;

[ApiController]
[Route("api/v01/[controller]/[action]")]
public class KnowledgeController(IServiceProvider serviceProvider) : ControllerBase
{
    private Lazy<IKnowledgeService> KnowledgeService => serviceProvider.Lazy<IKnowledgeService>();

    [HttpPost]
    public async Task<KnowledgeResponseModel> GetKnowledgeItems(KnowledgeRequestModel request)
    {
        return await KnowledgeService.Value.GetKnowledgeItems(request);
    }

    [HttpPost]
    [Authorize]
    public async Task<ActionResult<ContentModel>> AddContent(ContentCreateModel createModel)
    {
        try
        {
            var content = await KnowledgeService.Value.AddModifyContent(createModel);
            return CreatedAtAction(nameof(GetKnowledgeItems), new { }, content);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPut("{contentId}")]
    [Authorize]
    public async Task<IActionResult> UpdateContent(Guid contentId, ContentUpdateModel updateModel)
    {
        try
        {
            var content = await KnowledgeService.Value.AddModifyContent(contentId, updateModel);
            return Ok(content);
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

    [HttpDelete("{contentId}")]
    [Authorize]
    public async Task<IActionResult> DeactivateContent(Guid contentId)
    {
        // Note: In this versioning system, "delete" means deactivating the content
        // The service handles setting IsActive=false and RemovedTime
        try
        {
            await KnowledgeService.Value.DeactivateContent(contentId);
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
