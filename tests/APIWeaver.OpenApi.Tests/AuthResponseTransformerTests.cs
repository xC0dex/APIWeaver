namespace APIWeaver.OpenApi.Tests;

public class AuthResponseTransformerTests(WebApplicationFactory<Program> factory) : IClassFixture<WebApplicationFactory<Program>>
{
    [Fact]
    public async Task AddAuthResponse_ShouldAddAuthResponseToOperations_WhenCalled()
    {
        // Arrange
        var client = factory.CreateClient();

        // Act
        var response = await client.GetAsync("openapi/v1.json");
        await using var stream = await response.Content.ReadAsStreamAsync();
        var streamReader = new OpenApiStreamReader();
        var document = streamReader.Read(stream, out var diagnostic);
        
        // Assert
        diagnostic.Errors.Should().BeEmpty();

        var defaultPath = document.Paths["/api/default"];
        var responses = defaultPath.Operations.First().Value.Responses;
        responses.Should().HaveCount(1);
        responses.Should().ContainKey("200");
        
        var anonymousPath = document.Paths["/api/anonymous"];
        responses = anonymousPath.Operations.First().Value.Responses;
        responses.Should().HaveCount(1);
        responses.Should().ContainKey("200");
        
        var authorizePath = document.Paths["/api/authorize"];
        responses = authorizePath.Operations.First().Value.Responses;
        responses.Should().HaveCount(2);
        responses.Should().ContainKeys("200", "401");
        
        var rolePath = document.Paths["/api/role"];
        responses = rolePath.Operations.First().Value.Responses;
        responses.Should().HaveCount(3);
        responses.Should().ContainKeys("200", "401", "403");
    }
    
    [Fact]
    public async Task AddAuthResponse_ShouldAddAuthResponseToOperations_WhenCalledWithFallbackPolicy()
    {
        // Arrange
        var client = factory.WithWebHostBuilder(builder =>
        {
            builder.ConfigureTestServices(services =>
            {
                services.AddAuthorizationBuilder()
                    .AddFallbackPolicy("default", x => x.RequireAuthenticatedUser());
            });
        }).CreateClient();

        // Act
        var response = await client.GetAsync("openapi/v1.json");
        await using var stream = await response.Content.ReadAsStreamAsync();
        var streamReader = new OpenApiStreamReader();
        var document = streamReader.Read(stream, out var diagnostic);
        
        // Assert
        diagnostic.Errors.Should().BeEmpty();

        var defaultPath = document.Paths["/api/default"];
        var responses = defaultPath.Operations.First().Value.Responses;
        responses.Should().HaveCount(2);
        responses.Should().ContainKey("200", "401");
        
        var anonymousPath = document.Paths["/api/anonymous"];
        responses = anonymousPath.Operations.First().Value.Responses;
        responses.Should().HaveCount(1);
        responses.Should().ContainKey("200");
        
        var authorizePath = document.Paths["/api/authorize"];
        responses = authorizePath.Operations.First().Value.Responses;
        responses.Should().HaveCount(2);
        responses.Should().ContainKeys("200", "401");
        
        var rolePath = document.Paths["/api/role"];
        responses = rolePath.Operations.First().Value.Responses;
        responses.Should().HaveCount(3);
        responses.Should().ContainKeys("200", "401", "403");
    }
    
    [Fact]
    public async Task AddAuthResponse_ShouldAddAuthResponseToOperations_WhenCalledWithFallbackPolicyAndRequirements()
    {
        // Arrange
        var client = factory.WithWebHostBuilder(builder =>
        {
            builder.ConfigureTestServices(services =>
            {
                services.AddAuthorizationBuilder()
                    .AddFallbackPolicy("default", x => x.RequireRole("role"));
            });
        }).CreateClient();

        // Act
        var response = await client.GetAsync("openapi/v1.json");
        await using var stream = await response.Content.ReadAsStreamAsync();
        var streamReader = new OpenApiStreamReader();
        var document = streamReader.Read(stream, out var diagnostic);
        
        // Assert
        diagnostic.Errors.Should().BeEmpty();

        var defaultPath = document.Paths["/api/default"];
        var responses = defaultPath.Operations.First().Value.Responses;
        responses.Should().HaveCount(3);
        responses.Should().ContainKey("200", "401", "403");
        
        var anonymousPath = document.Paths["/api/anonymous"];
        responses = anonymousPath.Operations.First().Value.Responses;
        responses.Should().HaveCount(1);
        responses.Should().ContainKey("200");
        
        var authorizePath = document.Paths["/api/authorize"];
        responses = authorizePath.Operations.First().Value.Responses;
        responses.Should().HaveCount(2);
        responses.Should().ContainKeys("200", "401");
        
        var rolePath = document.Paths["/api/role"];
        responses = rolePath.Operations.First().Value.Responses;
        responses.Should().HaveCount(3);
        responses.Should().ContainKeys("200", "401", "403");
    }
    
    [Fact]
    public async Task AddAuthResponse_ShouldAddAuthResponseToOperations_WhenCalledWithDefaultPolicyAndRequirements()
    {
        // Arrange
        var client = factory.WithWebHostBuilder(builder =>
        {
            builder.ConfigureTestServices(services =>
            {
                services.AddAuthorizationBuilder()
                    .AddDefaultPolicy("default", x => x.RequireRole("role"));
            });
        }).CreateClient();

        // Act
        var response = await client.GetAsync("openapi/v1.json");
        await using var stream = await response.Content.ReadAsStreamAsync();
        var streamReader = new OpenApiStreamReader();
        var document = streamReader.Read(stream, out var diagnostic);
        
        // Assert
        diagnostic.Errors.Should().BeEmpty();

        var defaultPath = document.Paths["/api/default"];
        var responses = defaultPath.Operations.First().Value.Responses;
        responses.Should().HaveCount(1);
        responses.Should().ContainKey("200");
        
        var anonymousPath = document.Paths["/api/anonymous"];
        responses = anonymousPath.Operations.First().Value.Responses;
        responses.Should().HaveCount(1);
        responses.Should().ContainKey("200");
        
        var authorizePath = document.Paths["/api/authorize"];
        responses = authorizePath.Operations.First().Value.Responses;
        responses.Should().HaveCount(3);
        responses.Should().ContainKeys("200", "401", "403");
        
        var rolePath = document.Paths["/api/role"];
        responses = rolePath.Operations.First().Value.Responses;
        responses.Should().HaveCount(3);
        responses.Should().ContainKeys("200", "401", "403");
    }
}