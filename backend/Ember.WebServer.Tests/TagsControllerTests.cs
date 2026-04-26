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
public class TagsControllerTests
{
    private readonly WebApplicationFactory<Program> _factory;

    public TagsControllerTests(TestFixture factory)
    {
        _factory = factory;
    }

    [Fact]
    public async Task GetTags_ShouldReturnUnauthorized_WhenNotAuthenticated()
    {
        var client = _factory.CreateClient();
        var response = await client.GetAsync("/api/v01/tags");
        Assert.Equal(System.Net.HttpStatusCode.Unauthorized, response.StatusCode);
    }

    [Fact]
    public async Task CreateTag_ShouldReturnUnauthorized_WhenNotAuthenticated()
    {
        var client = _factory.CreateClient();
        var tag = new Tag { Name = "Test Tag" };
        var response = await client.PostAsJsonAsync("/api/v01/tags", tag);
        Assert.Equal(System.Net.HttpStatusCode.Unauthorized, response.StatusCode);
    }

    [Fact]
    public async Task UpdateTag_ShouldReturnUnauthorized_WhenNotAuthenticated()
    {
        var client = _factory.CreateClient();
        var id = Guid.NewGuid();
        var tag = new Tag { Name = "Updated Tag" };
        var response = await client.PutAsJsonAsync($"/api/v01/tags/{id}", tag);
        Assert.Equal(System.Net.HttpStatusCode.Unauthorized, response.StatusCode);
    }

    [Fact]
    public async Task DeleteTag_ShouldReturnUnauthorized_WhenNotAuthenticated()
    {
        var client = _factory.CreateClient();
        var id = Guid.NewGuid();
        var response = await client.DeleteAsync($"/api/v01/tags/{id}");
        Assert.Equal(System.Net.HttpStatusCode.Unauthorized, response.StatusCode);
    }
}