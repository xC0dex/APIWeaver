namespace APIWeaver.OpenApi.Tests;

public class SecuritySchemeTransformerTests(WebApplicationFactory<Program> factory) : IClassFixture<WebApplicationFactory<Program>>
{
    [Fact]
    public async Task AddSecurityScheme_ShouldAddSecuritySchemeToOperations_WhenCalled()
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
        var security = defaultPath.Operations.First().Value.Security;
        security.Should().BeNull();

        var authorizePath = document.Paths["/api/authorize"];
        security = authorizePath.Operations.First().Value.Security;
        security[0].Keys.First().Reference.Id.Should().Be("Bearer");

        var anonymousPath = document.Paths["/api/anonymous"];
        security = anonymousPath.Operations.First().Value.Security;
        security.Should().BeNull();

        var rolePath = document.Paths["/api/role"];
        security = rolePath.Operations.First().Value.Security;
        security[0].Keys.First().Reference.Id.Should().Be("Bearer");
    }

    [Fact]
    public async Task AddSecurityScheme_ShouldAddSecuritySchemeToOperations_WhenCalledWithFallbackPolicy()
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
        var security = defaultPath.Operations.First().Value.Security;
        security[0].Keys.First().Reference.Id.Should().Be("Bearer");

        var authorizePath = document.Paths["/api/authorize"];
        security = authorizePath.Operations.First().Value.Security;
        security[0].Keys.First().Reference.Id.Should().Be("Bearer");

        var anonymousPath = document.Paths["/api/anonymous"];
        security = anonymousPath.Operations.First().Value.Security;
        security.Should().BeNull();

        var rolePath = document.Paths["/api/role"];
        security = rolePath.Operations.First().Value.Security;
        security[0].Keys.First().Reference.Id.Should().Be("Bearer");
    }
}