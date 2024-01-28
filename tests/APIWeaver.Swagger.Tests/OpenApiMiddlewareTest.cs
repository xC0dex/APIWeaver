using APIWeaver.Extensions;
using Microsoft.OpenApi.Models;

namespace APIWeaver.Swagger.Tests;

public class OpenApiMiddlewareTest(WebApplicationFactory<Program> factory): IClassFixture<WebApplicationFactory<Program>>
{
    [Fact]
    public async Task Middleware_ShouldHandleRequest_WhenDocumentExists()
    {
        // Arrange
        var testFactory = factory.WithWebHostBuilder(b => b.ConfigureTestServices(services =>
            services.AddApiWeaver(options => options.OpenApiDocuments.Add("my-document", new OpenApiInfo
            {
                Title = "Hello world"
            }))));
        var client = testFactory.CreateClient();

        // Act
        var response = await client.GetAsync("/swagger/my-document-openapi.json");
        
        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }
}