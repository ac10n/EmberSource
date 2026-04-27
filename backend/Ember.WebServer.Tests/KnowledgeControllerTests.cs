using System.Net.Http.Json;
using Ember.Domain.Data;
using Ember.Domain.EmberEntities;
using Ember.Service;
using Ember.Service.Models;
using Ember.WebServer;
using Ember.WebServer.Areas.Knowledge.Models;
using Ember.Infrastructure;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace Ember.WebServer.Tests;

[Collection("TestCollection")]
public class KnowledgeControllerTests
{
    private readonly TestFixture _factory;

    public KnowledgeControllerTests(TestFixture factory)
    {
        _factory = factory;
    }

    [Fact]
    public async Task GetKnowledgeItems_ShouldReturnOk_WhenNotAuthenticated()
    {
        var client = _factory.CreateClient();
        var request = new KnowledgeRequestModel();
        var response = await client.PostAsJsonAsync("/api/v01/knowledge/getknowledgeitems", request);
        Assert.Equal(System.Net.HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task AddContent_ShouldReturnUnauthorized_WhenNotAuthenticated()
    {
        var client = _factory.CreateClient();
        var createModel = new ContentCreateModel
        {
            ContentTypeId = ContentTypes.Paragraph,
            ContentFormatId = ContentFormatEnum.Markdown,
            ContentVisibilityId = ContentVisibilityEnum.Public,
            Data = "Test content"
        };
        var response = await client.PostAsJsonAsync("/api/v01/knowledge/addcontent", createModel);
        Assert.Equal(System.Net.HttpStatusCode.Unauthorized, response.StatusCode);
    }

    [Fact]
    public async Task UpdateContent_ShouldReturnUnauthorized_WhenNotAuthenticated()
    {
        var client = _factory.CreateClient();
        var contentId = Guid.NewGuid();
        var updateModel = new ContentUpdateModel
        {
            ContentTypeId = ContentTypes.Paragraph,
            ContentFormatId = ContentFormatEnum.Markdown,
            ContentVisibilityId = ContentVisibilityEnum.Public,
            Data = "Updated content"
        };
        var response = await client.PutAsJsonAsync($"/api/v01/knowledge/updatecontent/{contentId}", updateModel);
        Assert.Equal(System.Net.HttpStatusCode.Unauthorized, response.StatusCode);
    }

    [Fact]
    public async Task DeactivateContent_ShouldReturnUnauthorized_WhenNotAuthenticated()
    {
        var client = _factory.CreateClient();
        var contentId = Guid.NewGuid();
        var response = await client.DeleteAsync($"/api/v01/knowledge/deactivatecontent/{contentId}");
        Assert.Equal(System.Net.HttpStatusCode.Unauthorized, response.StatusCode);
    }

    // TODO: Add authenticated E2E tests when authentication setup is available
}