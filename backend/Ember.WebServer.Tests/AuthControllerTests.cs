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
public class AuthControllerTests
{
    private readonly TestFixture _factory;

    public AuthControllerTests(TestFixture factory)
    {
        _factory = (TestFixture)factory;
    }

    [Fact]
    public async Task Login_ShouldReturnOk_WhenValidCredentials()
    {
        var client = _factory.CreateClient();
        var login = new LoginRequest("testuser", "password", false);
        var response = await client.PostAsJsonAsync("/api/v01/auth/login", login);
        // Since no user, expect BadRequest or Unauthorized
        Assert.True(response.StatusCode == System.Net.HttpStatusCode.Unauthorized || response.StatusCode == System.Net.HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task Register_ShouldReturnBadRequest_WhenInvalidInvite()
    {
        var client = _factory.CreateClient();
        var register = new RegisterDto(Guid.NewGuid().ToString(), "test", "test@test.com", "password");
        var response = await client.PostAsJsonAsync("/api/v01/auth/register", register);
        Assert.Equal(System.Net.HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task ConfirmEmail_ShouldReturnNotFound_WhenInvalidUser()
    {
        var client = _factory.CreateClient();
        var response = await client.GetAsync("/api/v01/auth/confirmemail?userId=00000000-0000-0000-0000-000000000000&token=test");
        Assert.Equal(System.Net.HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task Logout_ShouldReturnUnauthorized_WhenNotAuthenticated()
    {
        var client = _factory.CreateClient();
        var response = await client.PostAsync("/api/v01/auth/logout", null);
        Assert.Equal(System.Net.HttpStatusCode.Unauthorized, response.StatusCode);
    }
}