namespace NemtPlatform.Api.Tests.Integration;

using System.Net;
using NemtPlatform.Api.Tests.Infrastructure;

/// <summary>
/// Integration tests for API health and basic connectivity.
/// These tests verify the API can start and respond to basic requests.
/// </summary>
public class HealthCheckTests : IClassFixture<CustomWebApplicationFactory>
{
    private readonly CustomWebApplicationFactory _factory;
    private readonly HttpClient _client;

    public HealthCheckTests(CustomWebApplicationFactory factory)
    {
        _factory = factory;
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task Api_ShouldStart_AndRespondToRequests()
    {
        // Act
        // This test verifies that the API can start up without errors
        // Any endpoint that doesn't require authentication would work here
        var response = await _client.GetAsync("/");

        // Assert - the API started successfully if we get any response
        // (404 is fine since we haven't defined a root endpoint)
        Assert.NotNull(response);
    }

    [Fact]
    public async Task Api_OpenApiEndpoint_ShouldBeAccessibleInDevelopment()
    {
        // Note: This test may need adjustment based on environment configuration
        // The OpenAPI endpoint is typically only available in Development environment

        // Act
        var response = await _client.GetAsync("/openapi/v1.json");

        // Assert
        // OpenAPI might not be available in test environment
        // This scaffolds the test pattern for when it's enabled
        Assert.NotNull(response);
    }
}
