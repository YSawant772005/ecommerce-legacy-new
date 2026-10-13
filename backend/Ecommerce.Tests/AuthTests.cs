using Ecommerce.Infrastructure.Data;
using Microsoft.Extensions.DependencyInjection;
using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using Xunit;

namespace Ecommerce.Tests;

public class AuthTests : IClassFixture<TestWebApplicationFactory>
{
    private readonly HttpClient _client;

    public AuthTests(TestWebApplicationFactory factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task Register_WithValidData_ReturnsOkAndToken()
    {
        var request = new
        {
            fullName = "Integration User",
            email = $"int_{Guid.NewGuid():N}@example.com",
            password = "Test123!"
        };

        var response = await _client.PostAsJsonAsync("/api/auth/register", request);

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var result = await response.Content.ReadFromJsonAsync<JsonElement>();
        Assert.True(result.TryGetProperty("accessToken", out _), "Response should have accessToken");
        Assert.True(result.TryGetProperty("refreshToken", out _), "Response should have refreshToken");
    }

    [Fact]
    public async Task Register_DuplicateEmail_ReturnsBadRequest()
    {
        var request = new
        {
            fullName = "Duplicate User",
            email = "admin@novakart.com", // already seeded
            password = "Test123!"
        };

        var response = await _client.PostAsJsonAsync("/api/auth/register", request);
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task Login_WithValidCredentials_ReturnsToken()
    {
        var request = new { email = "admin@novakart.com", password = "Admin123!" };

        var response = await _client.PostAsJsonAsync("/api/auth/login", request);

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var result = await response.Content.ReadFromJsonAsync<JsonElement>();
        Assert.True(result.TryGetProperty("accessToken", out var token));
        Assert.False(string.IsNullOrWhiteSpace(token.GetString()));
    }

    [Fact]
    public async Task Login_WithWrongPassword_ReturnsUnauthorized()
    {
        var request = new { email = "admin@novakart.com", password = "WrongPassword!" };

        var response = await _client.PostAsJsonAsync("/api/auth/login", request);
        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }

    [Fact]
    public async Task Login_WithUnknownEmail_ReturnsUnauthorized()
    {
        var request = new { email = "nobody@example.com", password = "Test123!" };

        var response = await _client.PostAsJsonAsync("/api/auth/login", request);
        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }

    [Fact]
    public async Task GetMe_WithoutAuth_ReturnsUnauthorized()
    {
        var response = await _client.GetAsync("/api/auth/me");
        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }

    [Fact]
    public async Task GetMe_WithValidToken_ReturnsUserData()
    {
        // First login to get a token
        var loginResponse = await _client.PostAsJsonAsync("/api/auth/login",
            new { email = "admin@novakart.com", password = "Admin123!" });
        var loginData = await loginResponse.Content.ReadFromJsonAsync<JsonElement>();
        var token = loginData.GetProperty("accessToken").GetString();

        _client.DefaultRequestHeaders.Authorization =
            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

        var response = await _client.GetAsync("/api/auth/me");
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var user = await response.Content.ReadFromJsonAsync<JsonElement>();
        Assert.True(user.TryGetProperty("email", out var email));
        Assert.Equal("admin@novakart.com", email.GetString());

        // Clean up auth header
        _client.DefaultRequestHeaders.Authorization = null;
    }

    [Fact]
    public async Task RefreshToken_WithValidToken_ReturnsNewTokens()
    {
        // Login to get tokens
        var loginResponse = await _client.PostAsJsonAsync("/api/auth/login",
            new { email = "john@example.com", password = "Customer123!" });
        var loginData = await loginResponse.Content.ReadFromJsonAsync<JsonElement>();
        var refreshToken = loginData.GetProperty("refreshToken").GetString();

        var refreshResponse = await _client.PostAsJsonAsync("/api/auth/refresh",
            new { token = refreshToken });

        Assert.Equal(HttpStatusCode.OK, refreshResponse.StatusCode);
        var refreshData = await refreshResponse.Content.ReadFromJsonAsync<JsonElement>();
        Assert.True(refreshData.TryGetProperty("accessToken", out _));
    }
}
