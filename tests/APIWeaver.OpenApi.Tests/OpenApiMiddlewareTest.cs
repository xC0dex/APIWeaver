using System.Net;
using APIWeaver.Core;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;

namespace APIWeaver.OpenApi.Tests;

public class OpenApiMiddlewareTest(WebApplicationFactory<Program> factory) : IClassFixture<WebApplicationFactory<Program>>
{
    [Fact]
    public async Task Middleware_ShouldHandleRequest_WhenDocumentExists()
    {
        // Arrange
        var testFactory = factory.WithWebHostBuilder(b => b.ConfigureTestServices(services =>
            services.AddApiWeaver(options => options.OpenApiDocuments.Add("my-document", new OpenApiDocumentDefinition
            {
                Info = new OpenApiInfo
                {
                    Title = "Hello world"
                }
            }))));
        var client = testFactory.CreateClient();

        // Act
        var response = await client.GetAsync("/swagger/my-document-openapi.json");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    public async Task Middleware_ShouldAddDefaultOpenApiDocument_WhenNotAddedThroughSwaggerUiMiddleware()
    {
        // Arrange
        var client = factory.CreateClient();

        // Act
        var response = await client.GetAsync($"/swagger/{Constants.InitialDocumentName}-openapi.json");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    public async Task Middleware_ShouldReturnYaml_WhenRequested()
    {
        // Arrange
        var client = factory.CreateClient();

        // Act
        var response = await client.GetAsync($"/swagger/{Constants.InitialDocumentName}-openapi.yaml");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }
}