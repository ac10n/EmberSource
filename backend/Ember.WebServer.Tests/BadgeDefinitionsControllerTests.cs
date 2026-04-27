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
public class BadgeDefinitionsControllerTests
{
    private readonly WebApplicationFactory<Program> _factory;

    public BadgeDefinitionsControllerTests(TestFixture factory)
    {
        _factory = factory;
    }

    [Fact]
    public async Task GetBadgeDefinitions_ShouldReturnUnauthorized_WhenNotAuthenticated()
    {
        var client = _factory.CreateClient();
        var response = await client.GetAsync("/api/v01/badgedefinitions");
        Assert.Equal(System.Net.HttpStatusCode.Unauthorized, response.StatusCode);
    }

    [Fact]
    public async Task CreateBadgeDefinition_ShouldReturnUnauthorized_WhenNotAuthenticated()
    {
        var client = _factory.CreateClient();
        var badge = new BadgeDefinition { Name = "Test Badge", IsNumeric = true, MinValue = 0, MaxValue = 100 };
        var response = await client.PostAsJsonAsync("/api/v01/badgedefinitions", badge);
        Assert.Equal(System.Net.HttpStatusCode.Unauthorized, response.StatusCode);
    }

    [Fact]
    public async Task UpdateBadgeDefinition_ShouldReturnUnauthorized_WhenNotAuthenticated()
    {
        var client = _factory.CreateClient();
        var id = Guid.NewGuid();
        var badge = new BadgeDefinition { Name = "Updated Badge", IsNumeric = false };
        var response = await client.PutAsJsonAsync($"/api/v01/badgedefinitions/{id}", badge);
        Assert.Equal(System.Net.HttpStatusCode.Unauthorized, response.StatusCode);
    }

    [Fact]
    public async Task DeleteBadgeDefinition_ShouldReturnUnauthorized_WhenNotAuthenticated()
    {
        var client = _factory.CreateClient();
        var id = Guid.NewGuid();
        var response = await client.DeleteAsync($"/api/v01/badgedefinitions/{id}");
        Assert.Equal(System.Net.HttpStatusCode.Unauthorized, response.StatusCode);
    }
}