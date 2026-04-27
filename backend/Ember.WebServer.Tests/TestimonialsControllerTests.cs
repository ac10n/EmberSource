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
public class TestimonialsControllerTests
{
    private readonly TestFixture _factory;

    public TestimonialsControllerTests(TestFixture factory)
    {
        _factory = factory;
    }

    [Fact]
    public async Task GetTestimonials_ShouldReturnUnauthorized_WhenNotAuthenticated()
    {
        var client = _factory.CreateClient();
        var response = await client.GetAsync("/api/v01/testimonials");
        Assert.Equal(System.Net.HttpStatusCode.Unauthorized, response.StatusCode);
    }

    [Fact]
    public async Task GetTestimonial_ShouldReturnUnauthorized_WhenNotAuthenticated()
    {
        var client = _factory.CreateClient();
        var id = Guid.NewGuid();
        var response = await client.GetAsync($"/api/v01/testimonials/{id}");
        Assert.Equal(System.Net.HttpStatusCode.Unauthorized, response.StatusCode);
    }

    [Fact]
    public async Task CreateTestimonial_ShouldReturnUnauthorized_WhenNotAuthenticated()
    {
        var client = _factory.CreateClient();
        var createModel = new TestimonialCreateModel
        {
            ForEmberUserId = Guid.NewGuid(),
            BadgeDefinitionId = Guid.NewGuid(),
            FromTime = DateTimeOffset.UtcNow
        };
        var response = await client.PostAsJsonAsync("/api/v01/testimonials", createModel);
        Assert.Equal(System.Net.HttpStatusCode.Unauthorized, response.StatusCode);
    }

    [Fact]
    public async Task UpdateTestimonial_ShouldReturnUnauthorized_WhenNotAuthenticated()
    {
        var client = _factory.CreateClient();
        var id = Guid.NewGuid();
        var updateModel = new TestimonialUpdateModel
        {
            BadgeDefinitionId = Guid.NewGuid(),
            FromTime = DateTimeOffset.UtcNow
        };
        var response = await client.PutAsJsonAsync($"/api/v01/testimonials/{id}", updateModel);
        Assert.Equal(System.Net.HttpStatusCode.Unauthorized, response.StatusCode);
    }

    [Fact]
    public async Task DeleteTestimonial_ShouldReturnUnauthorized_WhenNotAuthenticated()
    {
        var client = _factory.CreateClient();
        var id = Guid.NewGuid();
        var response = await client.DeleteAsync($"/api/v01/testimonials/{id}");
        Assert.Equal(System.Net.HttpStatusCode.Unauthorized, response.StatusCode);
    }

    // TODO: Add authenticated E2E tests when authentication setup is available
}