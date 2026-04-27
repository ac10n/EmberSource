using System.Net.Http.Json;
using Ember.Domain.Data;
using Ember.Domain.EmberEntities;
using Ember.Service;
using Ember.Service.Models;
using Ember.WebServer;
using Ember.Infrastructure;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace Ember.WebServer.Tests;

[Collection("TestCollection")]
public class RelatedContentsControllerTests
{
    private readonly WebApplicationFactory<Program> _factory;

    public RelatedContentsControllerTests(TestFixture factory)
    {
        _factory = factory;
    }

    [Fact]
    public async Task GetRelatedContents_ShouldReturnUnauthorized_WhenNotAuthenticated()
    {
        var client = _factory.CreateClient();
        var response = await client.GetAsync("/api/v01/relatedcontents");
        Assert.Equal(System.Net.HttpStatusCode.Unauthorized, response.StatusCode);
    }

    [Fact]
    public async Task CreateRelatedContent_ShouldReturnUnauthorized_WhenNotAuthenticated()
    {
        var client = _factory.CreateClient();
        var relatedContent = new RelatedContent { ContentId = Guid.NewGuid(), RelatedContentId = Guid.NewGuid(), RelatedContentTypeId = RelatedContentTypeEnum.Reference };
        var response = await client.PostAsJsonAsync("/api/v01/relatedcontents", relatedContent);
        Assert.Equal(System.Net.HttpStatusCode.Unauthorized, response.StatusCode);
    }

    [Fact]
    public async Task UpdateRelatedContent_ShouldReturnUnauthorized_WhenNotAuthenticated()
    {
        var client = _factory.CreateClient();
        var id = Guid.NewGuid();
        var relatedContent = new RelatedContent { ContentId = Guid.NewGuid(), RelatedContentId = Guid.NewGuid(), RelatedContentTypeId = RelatedContentTypeEnum.Reference };
        var response = await client.PutAsJsonAsync($"/api/v01/relatedcontents/{id}", relatedContent);
        Assert.Equal(System.Net.HttpStatusCode.Unauthorized, response.StatusCode);
    }

    [Fact]
    public async Task DeleteRelatedContent_ShouldReturnUnauthorized_WhenNotAuthenticated()
    {
        var client = _factory.CreateClient();
        var id = Guid.NewGuid();
        var response = await client.DeleteAsync($"/api/v01/relatedcontents/{id}");
        Assert.Equal(System.Net.HttpStatusCode.Unauthorized, response.StatusCode);
    }
}