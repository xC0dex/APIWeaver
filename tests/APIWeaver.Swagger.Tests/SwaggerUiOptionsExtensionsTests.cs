namespace APIWeaver.Swagger.Tests;

public class SwaggerUiOptionsExtensionsTests
{
    [Fact]
    public void Extensions_ShouldSetProperties()
    {
        // Arrange
        var options = new SwaggerOptions();

        // Act
        options
            .AddOpenApiDocument("name", "endpoint")
            .WithDeepLinking(true)
            .WithDisplayOperationId(true)
            .WithDefaultModelExpandDepth(2)
            .WithDefaultModelsExpandDepth(2)
            .WithDisplayRequestDuration(true)
            .WithMaxDisplayedTags(4)
            .WithShowExtensions(true)
            .WithShowExtensions(true)
            .WithShowCommonExtensions(true)
            .WithTryItOut(true)
            .WithRequestSnippets(true)
            .WithOAuth2RedirectUrl("custom-html")
            .WithValidatorUrl("url");

        // Assert
        options.Urls.Should().BeEquivalentTo([new Url("name", "endpoint")]);
        options.DeepLinking.Should().BeTrue();
        options.DisplayOperationId.Should().BeTrue();
        options.DefaultModelExpandDepth.Should().Be(2);
        options.DefaultModelsExpandDepth.Should().Be(2);
        options.DisplayRequestDuration.Should().BeTrue();
        options.MaxDisplayedTags.Should().Be(4);
        options.ShowExtensions.Should().BeTrue();
        options.ShowCommonExtensions.Should().BeTrue();
        options.TryItOutEnabled.Should().BeTrue();
        options.RequestSnippetsEnabled.Should().BeTrue();
        options.OAuth2RedirectUrl.Should().Be("custom-html");
        options.ValidatorUrl.Should().Be("url");
    }
}