namespace APIWeaver.OpenApi.Tests;

public class OpenApiServersGeneratorTests
{
    private readonly IOptions<OpenApiOptions> _options = Substitute.For<IOptions<OpenApiOptions>>();
    private readonly IServiceProvider _serviceProvider = Substitute.For<IServiceProvider>();
    private OpenApiServersGenerator _generator;

    public OpenApiServersGeneratorTests()
    {
        _generator = new OpenApiServersGenerator(_options, _serviceProvider);
    }

    [Fact]
    public async Task GenerateServersAsync_ShouldReturnDefaultServer_WhenNoTransformers()
    {
        _options.Value.Returns(new OpenApiOptions());

        var result = await _generator.GenerateServersAsync("http://localhost/api", CancellationToken.None);

        result.Should().HaveCount(1);
        result[0].Url.Should().Be("http://localhost/api");
        result[0].Description.Should().BeNull();
    }

    [Fact]
    public async Task GenerateServersAsync_ShouldTransformDefaultServer_WhenTransformerProvided()
    {
        const string description = "transformed";
        var generatorOptions = new OpenApiGeneratorOptions();
        generatorOptions.ServerTransformers.Add(async context =>
        {
            context.OpenApiServer.Description = description;
            await Task.Delay(69);
        });
        _options.Value.Returns(new OpenApiOptions {GeneratorOptions = generatorOptions});
        _generator = new OpenApiServersGenerator(_options, _serviceProvider);

        var result = await _generator.GenerateServersAsync("http://localhost/api", CancellationToken.None);

        result.Should().HaveCount(1);
        result[0].Description.Should().Be(description);
    }

    [Fact]
    public async Task GenerateServersAsync_ShouldAddCustomServer_WhenProvided()
    {
        const string url = "http://localhost";
        var openApiServer = new OpenApiServer
        {
            Url = url
        };
        _options.Value.Returns(new OpenApiOptions {Servers = [openApiServer]});
        _generator = new OpenApiServersGenerator(_options, _serviceProvider);

        var result = await _generator.GenerateServersAsync("http://localhost/api", CancellationToken.None);

        result.Should().HaveCount(2);
        result[1].Url.Should().Be(url);
    }
}