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
public class ContentInteractionsControllerTests
{
    private readonly WebApplicationFactory<Program> _factory;

    public ContentInteractionsControllerTests(TestFixture factory)
    {
        _factory = factory;
    }

    [Fact]
    public async Task GetInteractions_ShouldReturnUnauthorized_WhenNotAuthenticated()
    {
        var client = _factory.CreateClient();
        var response = await client.GetAsync("/api/v01/contentinteractions");
        Assert.Equal(System.Net.HttpStatusCode.Unauthorized, response.StatusCode);
    }

    [Fact]
    public async Task CreateInteraction_ShouldReturnUnauthorized_WhenNotAuthenticated()
    {
        var client = _factory.CreateClient();
        var interaction = new ContentInteraction { ContentId = Guid.NewGuid(), IsRead = true };
        var response = await client.PostAsJsonAsync("/api/v01/contentinteractions", interaction);
        Assert.Equal(System.Net.HttpStatusCode.Unauthorized, response.StatusCode);
    }

    [Fact]
    public async Task UpdateInteraction_ShouldReturnUnauthorized_WhenNotAuthenticated()
    {
        var client = _factory.CreateClient();
        var id = Guid.NewGuid();
        var interaction = new ContentInteraction { ContentId = Guid.NewGuid(), IsRead = false };
        var response = await client.PutAsJsonAsync($"/api/v01/contentinteractions/{id}", interaction);
        Assert.Equal(System.Net.HttpStatusCode.Unauthorized, response.StatusCode);
    }

    [Fact]
    public async Task DeleteInteraction_ShouldReturnUnauthorized_WhenNotAuthenticated()
    {
        var client = _factory.CreateClient();
        var id = Guid.NewGuid();
        var response = await client.DeleteAsync($"/api/v01/contentinteractions/{id}");
        Assert.Equal(System.Net.HttpStatusCode.Unauthorized, response.StatusCode);
    }
}