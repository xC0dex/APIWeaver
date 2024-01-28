using APIWeaver.OpenApi.Extensions;
using APIWeaver.OpenApi.Models;
using APIWeaver.Swagger.Helper;
using Microsoft.OpenApi.Models;

namespace APIWeaver.Swagger.Tests;

public sealed class SwaggerConfigurationMiddlewareTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient _client;
    private readonly WebApplicationFactory<Program> _factory;


    public SwaggerConfigurationMiddlewareTests(WebApplicationFactory<Program> factory)
    {
        _factory = factory.WithWebHostBuilder(b => b.ConfigureTestServices(x => x.Configure<SwaggerOptions>(configuration =>
        {
            configuration.Title = "My Swagger UI";
            configuration.WithUiOptions(o =>
            {
                o.DeepLinking = true;
                o.DisplayOperationId = true;
                o.DefaultModelsExpandDepth = 2;
                o.DefaultModelExpandDepth = 2;
                o.DisplayRequestDuration = true;
                o.MaxDisplayedTags = 5;
                o.ShowExtensions = true;
                o.ShowCommonExtensions = true;
                o.TryItOutEnabled = true;
                o.RequestSnippetsEnabled = true;
                o.OAuth2RedirectUrl = "my-oauth2-redirect.html";
                o.ValidatorUrl = "my-validator-url";
            });
            configuration.WithOAuth2Options(o =>
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
        _client = _factory.CreateClient();
    }

    [Fact]
    public async Task Middleware_ShouldNotBeCalled_WhenEndpointPrefixNotRequested()
    {
        // Act
        var response = await _client.GetAsync("/");
        var content = await response.Content.ReadAsStringAsync();

        // Assert
        content.Should().Be("Hello World!");
    }

    [Fact]
    public async Task Middleware_ShouldRedirect_WhenEndpointPrefixRequested()
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
    public async Task Middleware_ShouldReturnIndex_WhenRequested()
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
    public async Task Middleware_ShouldReturnJs_WhenRequested()
    {
        // Act
        var response = await _client.GetAsync("/swagger/swagger-initializer.js");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var content = await response.Content.ReadAsStringAsync();

        const string expected = """
                                fetch('./configuration.json')
                                    .then(response => response.json())
                                    .then(({title, uiOptions, additionalUiOptions, oAuth2Options}) => {
                                        document.title = title;
                                        if (additionalUiOptions.darkMode) {
                                            appendHeaderContent('<link rel="stylesheet" type="text/css" href="./dark-mode.css" />');
                                        }
                                        uiOptions.dom_id = '#swagger-ui';
                                        uiOptions.presets = [SwaggerUIBundle.presets.apis, SwaggerUIStandalonePreset];
                                        uiOptions.plugins = [SwaggerUIBundle.plugins.DownloadUrl];
                                        uiOptions.layout = 'StandaloneLayout';
                                        window.ui = SwaggerUIBundle(uiOptions);
                                        oAuth2Options && window.ui.initOAuth(oAuth2Options);
                                    });
                                
                                function appendHeaderContent(content) {
                                    document.getElementsByTagName('head')[0].insertAdjacentHTML('beforeend', content);
                                }
                                """;
        content.ReplaceLineEndings().Should().Be(expected);
    }

    [Fact]
    public async Task Middleware_ShouldReturnValidConfiguration_WhenRequested()
    {
        // Act
        var response = await _client.GetAsync("/swagger/configuration.json");
        await using var content = await response.Content.ReadAsStreamAsync();
        var configuration = (await JsonSerializer.DeserializeAsync<SwaggerOptions>(content, JsonSerializerHelper.SerializerOptions))!;

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        configuration.Title.Should().Be("My Swagger UI");
        configuration.EndpointPrefix.Should().Be("swagger");

        configuration.UiOptions.DeepLinking.Should().BeTrue();
        configuration.UiOptions.DisplayOperationId.Should().BeTrue();
        configuration.UiOptions.DefaultModelsExpandDepth.Should().Be(2);
        configuration.UiOptions.DefaultModelExpandDepth.Should().Be(2);
        configuration.UiOptions.DisplayRequestDuration.Should().BeTrue();
        configuration.UiOptions.MaxDisplayedTags.Should().Be(5);
        configuration.UiOptions.ShowExtensions.Should().BeTrue();
        configuration.UiOptions.ShowCommonExtensions.Should().BeTrue();
        configuration.UiOptions.TryItOutEnabled.Should().BeTrue();
        configuration.UiOptions.RequestSnippetsEnabled.Should().BeTrue();
        configuration.UiOptions.OAuth2RedirectUrl.Should().Be("my-oauth2-redirect.html");
        configuration.UiOptions.ValidatorUrl.Should().Be("my-validator-url");

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
    public async Task Middleware_ShouldAddSwaggerDocument_WhenOnlyOpenApiDocumentProvided()
    {
        // Arrange
        var testFactory = _factory.WithWebHostBuilder(b => b.ConfigureTestServices(services =>
            services.AddApiWeaver(options => options.OpenApiDocuments.Add("my-document", new OpenApiDocumentDefinition
            {
                Info = new OpenApiInfo
                {
                    Title = "Hello world"
                }
            }))));
        var client = testFactory.CreateClient();

        // Act
        var response = await client.GetAsync("/swagger/configuration.json");
        await using var content = await response.Content.ReadAsStreamAsync();
        var configuration = (await JsonSerializer.DeserializeAsync<SwaggerOptions>(content, JsonSerializerHelper.SerializerOptions))!;
        var urls = configuration.UiOptions.Urls;

        // Assert
        urls.Should().HaveCount(1);
        urls[0].Name.Should().Be("my-document");
        urls[0].Endpoint.Should().Be("/swagger/my-document-openapi.json");
    }

    [Fact]
    public async Task Middleware_ShouldThrowException_WhenDocumentMismatch()
    {
        // Arrange
        var testFactory = _factory.WithWebHostBuilder(b => b.ConfigureTestServices(services => services.Configure<SwaggerOptions>(o => o.WithOpenApiDocument("my-document"))));
        var client = testFactory.CreateClient();

        // Act
        var response = await client.GetAsync("/swagger/configuration.json");
        var content = await response.Content.ReadAsStringAsync();

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.InternalServerError);
        content.Should().Contain("APIWeaver.Swagger.Exceptions.OpenApiDocumentMismatchException");
    }

    [Fact]
    public async Task Middleware_ShouldHandleCustomEndpoint_WhenEndpointPrefixModified()
    {
        // Arrange
        const string endpointPrefix = "open-api";
        var testFactory = _factory.WithWebHostBuilder(b => b.ConfigureTestServices(services => services.Configure<SwaggerOptions>(o => o.EndpointPrefix = $"/{endpointPrefix}/")));
        var client = testFactory.CreateClient();

        // Act
        var response = await client.GetAsync($"/{endpointPrefix}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }
}