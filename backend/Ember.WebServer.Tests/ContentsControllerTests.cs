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
using Microsoft.Extensions.Configuration;
using Xunit;

namespace Ember.WebServer.Tests;

[Collection("TestCollection")]
public class ContentsControllerTests
{
    private readonly WebApplicationFactory<Program> _factory;

    public ContentsControllerTests(TestFixture factory)
    {
        _factory = factory;
        var testFixture = (TestFixture)_factory;
        testFixture.SeedRequiredData();
    }

    [Fact]
    public async Task CreateContent_ShouldReturnUnauthorized_WhenNotAuthenticated()
    {
        // Arrange
        var client = _factory.CreateClient();
        var content = new Content
        {
            Identifier = Guid.NewGuid(),
            Version = 1,
            Title = "Test Content",
            Data = "Test data",
            ContentTypeId = ContentTypes.Paragraph,
            ContentFormatId = ContentFormatEnum.Markdown,
            FormatVersion = 1,
            ContentVisibilityId = ContentVisibilityEnum.Public
        };

        // Act
        var response = await client.PostAsJsonAsync("/api/v01/contents", content);

        // Assert
        Assert.Equal(System.Net.HttpStatusCode.Unauthorized, response.StatusCode);
    }

    [Fact]
    public async Task GetContents_ShouldReturnUnauthorized_WhenNotAuthenticated()
    {
        // Arrange
        var client = _factory.CreateClient();

        // Act
        var response = await client.GetAsync("/api/v01/contents");

        // Assert
        Assert.Equal(System.Net.HttpStatusCode.Unauthorized, response.StatusCode);
    }

    [Fact]
    public async Task GetContent_ShouldReturnUnauthorized_WhenNotAuthenticated()
    {
        // Arrange
        var client = _factory.CreateClient();
        var id = Guid.NewGuid();

        // Act
        var response = await client.GetAsync($"/api/v01/contents/{id}");

        // Assert
        Assert.Equal(System.Net.HttpStatusCode.Unauthorized, response.StatusCode);
    }

    [Fact]
    public async Task UpdateContent_ShouldReturnUnauthorized_WhenNotAuthenticated()
    {
        // Arrange
        var client = _factory.CreateClient();
        var id = Guid.NewGuid();
        var content = new Content
        {
            Identifier = Guid.NewGuid(),
            Version = 1,
            Title = "Updated Content",
            Data = "Updated data",
            ContentTypeId = ContentTypes.Paragraph,
            ContentFormatId = ContentFormatEnum.Markdown,
            FormatVersion = 1,
            ContentVisibilityId = ContentVisibilityEnum.Public
        };

        // Act
        var response = await client.PutAsJsonAsync($"/api/v01/contents/{id}", content);

        // Assert
        Assert.Equal(System.Net.HttpStatusCode.Unauthorized, response.StatusCode);
    }

    [Fact]
    public async Task DeleteContent_ShouldReturnUnauthorized_WhenNotAuthenticated()
    {
        // Arrange
        var client = _factory.CreateClient();
        var id = Guid.NewGuid();

        // Act
        var response = await client.DeleteAsync($"/api/v01/contents/{id}");

        // Assert
        Assert.Equal(System.Net.HttpStatusCode.Unauthorized, response.StatusCode);
    }
}