namespace APIWeaver.Swagger.Tests;

public class SwaggerUiOptionsExtensionsTests
{
    [Fact]
    public void MethodName_ShouldShould_WhenWhen()
    {
        // Arrange
        var options = new SwaggerUiOptions();

        // Act
        options
            .AddUrl(new Url("name", "endpoint"))
            .WithDeepLinking(true)
            .WithDisplayOperationId(true)
            .WithDefaultModelExpandDepth(2)
            .WithDefaultModelsExpandDepth(2)
            .WithMaxDisplayedTags(4)
            .WithShowExtensions(true)
            .WithShowExtensions(true)
            .WithShowCommonExtensions(true)
            .WithTryItOutEnabled(true)
            .WithRequestSnippetsEnabled(true)
            .WithOAuth2RedirectUrl("custom-html")
            .WithValidatorUrl("url");

        // Assert
        options.Urls.Should().BeEquivalentTo([new Url("name", "endpoint")]);
        options.DeepLinking.Should().Be(true);
        options.DisplayOperationId.Should().Be(true);
        options.DefaultModelExpandDepth.Should().Be(2);
        options.DefaultModelsExpandDepth.Should().Be(2);
        options.MaxDisplayedTags.Should().Be(4);
        options.ShowExtensions.Should().Be(true);
        options.ShowCommonExtensions.Should().Be(true);
        options.TryItOutEnabled.Should().Be(true);
        options.RequestSnippetsEnabled.Should().Be(true);
        options.OAuth2RedirectUrl.Should().Be("custom-html");
        options.ValidatorUrl.Should().Be("url");
    }
}