using System.Text.Json.Serialization;

namespace APIWeaver.Swagger.Tests;

public sealed class SwaggerEndpointTests : IClassFixture<WebApplicationFactory<Program>>
{
    private static readonly JsonSerializerOptions Options = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
    };

    private readonly HttpClient _client;
    private readonly WebApplicationFactory<Program> _factory;

    public SwaggerEndpointTests(WebApplicationFactory<Program> factory)
    {
        _factory = factory;
        _client = _factory.CreateClient();
    }

    [Fact]
    public async Task Endpoint_ShouldNotBeCalled_WhenEndpointPrefixNotRequested()
    {
        // Act
        var response = await _client.GetAsync("/");
        var content = await response.Content.ReadAsStringAsync();

        // Assert
        content.Should().Be("Hello World!");
    }

    [Fact]
    public async Task Endpoint_ShouldRedirect_WhenEndpointPrefixRequested()
    {
        // Arrange
        var client = _factory.CreateClient(new WebApplicationFactoryClientOptions
        {
            AllowAutoRedirect = false
        });
        // Act
        var response = await client.GetAsync("/swagger");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Found);
        response.Headers.Location.Should().Be("/swagger/index.html");
    }

    [Fact]
    public async Task Endpoint_ShouldReturnIndex_WhenRequested()
    {
        // Act
        var response = await _client.GetAsync("/swagger");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var content = await response.Content.ReadAsStringAsync();

        const string expected = """
                                <!-- HTML for static distribution bundle build -->
                                <!DOCTYPE html>
                                <html lang="en">
                                  <head>
                                    <meta charset="UTF-8">
                                    <title>Swagger UI</title>
                                    <link rel="stylesheet" type="text/css" href="./swagger-ui.css" />
                                    <link rel="stylesheet" type="text/css" href="index.css" />
                                    <link rel="icon" type="image/png" href="./favicon-32x32.png" sizes="32x32" />
                                    <link rel="icon" type="image/png" href="./favicon-16x16.png" sizes="16x16" />
                                  </head>
                                
                                  <body>
                                    <div id="swagger-ui"></div>
                                    <script src="./swagger-ui-bundle.js" charset="UTF-8"> </script>
                                    <script src="./swagger-ui-standalone-preset.js" charset="UTF-8"> </script>
                                    <script src="./swagger-initializer.js" charset="UTF-8"> </script>
                                  </body>
                                </html>

                                """;
        content.ReplaceLineEndings().Should().Be(expected);
    }

    [Fact]
    public async Task Endpoint_ShouldReturnNotFound_WhenFileDoesNotExist()
    {
        // Arrange
        var client = _factory.CreateClient();

        // Act
        var response = await client.GetAsync("/swagger/no-file");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task Endpoint_ShouldReturnNawdotFound_WhenFileDoesNotExist()
    {
        // Act
        var response = await _client.GetAsync("/swagger/configuration.json");
        await using var content = await response.Content.ReadAsStreamAsync();
        var configuration = await JsonSerializer.DeserializeAsync<SwaggerOptions>(content, Options);

        // Assert
        configuration!.Title.Should().Be("APIWeaver.Swagger.TestApi | Swagger UI");
    }

    [Fact]
    public async Task Endpoint_ShouldReturnValidConfiguration_WhenRequested()
    {
        // Arrange
        var factory = _factory.WithWebHostBuilder(b => b.ConfigureTestServices(x => x.Configure<SwaggerOptions>(configuration =>
        {
            configuration
                .WithTitle("My Swagger UI")
                .WithDeepLinking(true)
                .WithDisplayOperationId(true)
                .WithDefaultModelsExpandDepth(2)
                .WithDefaultModelExpandDepth(2)
                .WithDisplayRequestDuration(true)
                .WithMaxDisplayedTags(5)
                .WithShowExtensions(true)
                .WithShowCommonExtensions(true)
                .WithTryItOut(true)
                .WithRequestSnippets(true)
                .WithOAuth2RedirectUrl("my-oauth2-redirect.html")
                .WithValidatorUrl("my-validator-url")
                .WithDarkMode(false)
                .AddScript("my-script.js")
                .AddStylesheet("my-stylesheet.css")
                .WithOAuth2Options(o =>
                {
                    o.ClientId = "my-client-id";
                    o.ClientSecret = "my-client-secret";
                    o.Realm = "my-realm";
                    o.AppName = "my-app-name";
                    o.ScopeSeparator = " ";
                    o.Scopes = ["offline"];
                    o.AdditionalQueryStringParams = new Dictionary<string, string> {{"audience", "my-audience"}};
                    o.UseBasicAuthenticationWithAccessCodeGrant = true;
                });
        })));
        var client = factory.CreateClient();

        // Act
        var response = await client.GetAsync("/swagger/configuration.json");
        await using var content = await response.Content.ReadAsStreamAsync();

        var configuration = await JsonSerializer.DeserializeAsync<SwaggerOptions>(content, Options);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        configuration.Should().NotBeNull();
        configuration!.Title.Should().Be("My Swagger UI");
        configuration.RoutePrefix.Should().Be("swagger");
        configuration.DarkMode.Should().BeFalse();
        configuration.Scripts.Should().Contain("my-script.js");
        configuration.Stylesheets.Should().Contain("my-stylesheet.css");

        configuration.DeepLinking.Should().BeTrue();
        configuration.DisplayOperationId.Should().BeTrue();
        configuration.DefaultModelsExpandDepth.Should().Be(2);
        configuration.DefaultModelExpandDepth.Should().Be(2);
        configuration.DisplayRequestDuration.Should().BeTrue();
        configuration.MaxDisplayedTags.Should().Be(5);
        configuration.ShowExtensions.Should().BeTrue();
        configuration.ShowCommonExtensions.Should().BeTrue();
        configuration.TryItOutEnabled.Should().BeTrue();
        configuration.RequestSnippetsEnabled.Should().BeTrue();
        configuration.OAuth2RedirectUrl.Should().Be("my-oauth2-redirect.html");
        configuration.ValidatorUrl.Should().Be("my-validator-url");

        configuration.OAuth2Options.Should().NotBeNull();
        configuration.OAuth2Options!.ClientId.Should().Be("my-client-id");
        configuration.OAuth2Options.ClientSecret.Should().Be("my-client-secret");
        configuration.OAuth2Options.Realm.Should().Be("my-realm");
        configuration.OAuth2Options.AppName.Should().Be("my-app-name");
        configuration.OAuth2Options.ScopeSeparator.Should().Be(" ");
        configuration.OAuth2Options.Scopes.Should().Contain("offline");
        configuration.OAuth2Options.AdditionalQueryStringParams.Should().Contain("audience", "my-audience");
        configuration.OAuth2Options.UseBasicAuthenticationWithAccessCodeGrant.Should().BeTrue();
    }

    [Fact]
    public async Task Endpoint_ShouldAddSwaggerDocument_WhenOnlyOpenApiDocumentProvided()
    {
        // Arrange
        var testFactory = _factory.WithWebHostBuilder(b => b.ConfigureTestServices(services =>
            services.AddOpenApiDocument("my-document")));
        var client = testFactory.CreateClient();

        // Act
        var response = await client.GetAsync("/swagger/configuration.json");
        await using var content = await response.Content.ReadAsStreamAsync();
        var configuration = await JsonSerializer.DeserializeAsync<SwaggerOptions>(content, Options);
        var urls = configuration!.Urls;

        // Assert
        urls.Should().HaveCount(1);
        urls[0].Name.Should().Be("my-document");
        urls[0].Route.Should().Be("/openapi/my-document.json");
    }

    [Fact]
    public async Task Endpoint_ShouldHandleCustomEndpoint_WhenEndpointPrefixModified()
    {
        // Arrange
        const string endpointPrefix = "open-api";
        var testFactory = _factory.WithWebHostBuilder(b => b.ConfigureTestServices(services => services.Configure<SwaggerOptions>(o => o.RoutePrefix = $"/{endpointPrefix}/")));
        var client = testFactory.CreateClient();

        // Act
        var response = await client.GetAsync($"/{endpointPrefix}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }
}