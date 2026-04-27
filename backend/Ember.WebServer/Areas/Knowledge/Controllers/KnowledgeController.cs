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

    [HttpGet]
    [Authorize]
    public async Task<IEnumerable<TagModel>> GetTags()
    {
        return await KnowledgeService.Value.GetTags();
    }

    [HttpGet("{tagId}")]
    [Authorize]
    public async Task<ActionResult<TagModel>> GetTag(Guid tagId)
    {
        var tag = await KnowledgeService.Value.GetTag(tagId);
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
            var tag = await KnowledgeService.Value.CreateTag(createModel);
            return CreatedAtAction(nameof(GetTag), new { tagId = tag.Id }, tag);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPut("{tagId}")]
    [Authorize]
    public async Task<IActionResult> UpdateTag(Guid tagId, TagUpdateModel updateModel)
    {
        try
        {
            var tag = await KnowledgeService.Value.UpdateTag(tagId, updateModel);
            return Ok(tag);
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

    [HttpDelete("{tagId}")]
    [Authorize]
    public async Task<IActionResult> DeleteTag(Guid tagId)
    {
        try
        {
            await KnowledgeService.Value.DeleteTag(tagId);
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

    [HttpGet]
    [Authorize]
    public async Task<IEnumerable<CollectionModel>> GetCollections()
    {
        return await KnowledgeService.Value.GetCollections();
    }

    [HttpGet("{collectionId}")]
    [Authorize]
    public async Task<ActionResult<CollectionModel>> GetCollection(Guid collectionId)
    {
        var collection = await KnowledgeService.Value.GetCollection(collectionId);
        if (collection == null)
        {
            return NotFound();
        }

        return Ok(collection);
    }

    [HttpPost]
    [Authorize]
    public async Task<ActionResult<CollectionModel>> CreateCollection(CollectionCreateModel createModel)
    {
        try
        {
            var collection = await KnowledgeService.Value.CreateCollection(createModel);
            return CreatedAtAction(nameof(GetCollection), new { collectionId = collection.Id }, collection);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPut("{collectionId}")]
    [Authorize]
    public async Task<IActionResult> UpdateCollection(Guid collectionId, CollectionUpdateModel updateModel)
    {
        try
        {
            var collection = await KnowledgeService.Value.UpdateCollection(collectionId, updateModel);
            return Ok(collection);
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

    [HttpDelete("{collectionId}")]
    [Authorize]
    public async Task<IActionResult> DeleteCollection(Guid collectionId)
    {
        try
        {
            await KnowledgeService.Value.DeleteCollection(collectionId);
            return NoContent();
        }
        catch (UnauthorizedAccessException)
        {
            return Forbid();
        }
    }

    [HttpPost("{collectionId}/items")]
    [Authorize]
    public async Task<IActionResult> AddContentToCollection(Guid collectionId, CollectionItemCreateModel itemModel)
    {
        try
        {
            await KnowledgeService.Value.AddContentToCollection(collectionId, itemModel);
            return NoContent();
        }
        catch (UnauthorizedAccessException)
        {
            return Forbid();
        }
    }

    [HttpDelete("{collectionId}/items/{contentId}")]
    [Authorize]
    public async Task<IActionResult> RemoveContentFromCollection(Guid collectionId, Guid contentId)
    {
        try
        {
            await KnowledgeService.Value.RemoveContentFromCollection(collectionId, contentId);
            return NoContent();
        }
        catch (UnauthorizedAccessException)
        {
            return Forbid();
        }
    }
}
