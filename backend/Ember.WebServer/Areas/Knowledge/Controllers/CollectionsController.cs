using Ember.WebServer.Areas.Knowledge.Models;
using Ember.WebServer.Areas.Knowledge.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Ember.WebServer.Areas.Knowledge.Controllers;

[ApiController]
[Route("api/v01/[controller]")]
[Authorize]
public class CollectionsController(IKnowledgeService knowledgeService) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IEnumerable<CollectionModel>>> GetCollections()
    {
        var collections = await knowledgeService.GetCollections();
        return Ok(collections);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<CollectionModel>> GetCollection(Guid id)
    {
        var collection = await knowledgeService.GetCollection(id);

        if (collection == null)
        {
            return NotFound();
        }

        return Ok(collection);
    }

    [HttpPost]
    public async Task<ActionResult<CollectionModel>> CreateCollection(CollectionCreateModel createModel)
    {
        try
        {
            var collection = await knowledgeService.CreateCollection(createModel);
            return CreatedAtAction(nameof(GetCollection), new { id = collection.Id }, collection);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateCollection(Guid id, CollectionUpdateModel updateModel)
    {
        try
        {
            var collection = await knowledgeService.UpdateCollection(id, updateModel);
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

    [HttpPost("{collectionId}/items")]
    public async Task<IActionResult> AddContentToCollection(Guid collectionId, CollectionItemCreateModel itemModel)
    {
        try
        {
            await knowledgeService.AddContentToCollection(collectionId, itemModel);
            return NoContent();
        }
        catch (UnauthorizedAccessException)
        {
            return Forbid();
        }
    }

    [HttpDelete("{collectionId}/items/{contentId}")]
    public async Task<IActionResult> RemoveContentFromCollection(Guid collectionId, Guid contentId)
    {
        try
        {
            await knowledgeService.RemoveContentFromCollection(collectionId, contentId);
            return NoContent();
        }
        catch (UnauthorizedAccessException)
        {
            return Forbid();
        }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteCollection(Guid id)
    {
        try
        {
            await knowledgeService.DeleteCollection(id);
            return NoContent();
        }
        catch (UnauthorizedAccessException)
        {
            return Forbid();
        }
    }
}