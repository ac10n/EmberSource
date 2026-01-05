using Ember.WebServer.Areas.Knowledge.Models;
using Ember.WebServer.Areas.Knowledge.Services;
using Ember.WebServer.Helpers;

namespace Ember.WebServer.Areas.Knowledge.Controllers;

[ApiController]
public class KnowledgeController(IServiceProvider serviceProvider) : ControllerBase
{
    private Lazy<IKnowledgeService> KnowledgeService => serviceProvider.Lazy<IKnowledgeService>();

    [HttpGet]
    public async Task<KnowledgeResponseModel> GetKnowledgeItems(KnowledgeRequestModel request)
    {
        return await KnowledgeService.Value.GetKnowledgeItems(request);
    }
}
