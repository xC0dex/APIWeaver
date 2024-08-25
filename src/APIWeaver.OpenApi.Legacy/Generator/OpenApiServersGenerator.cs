namespace APIWeaver.OpenApi.Generator;

internal sealed class OpenApiServersGenerator(IOptions<OpenApiOptions> options, IServiceProvider serviceProvider) : IOpenApiServersGenerator
{
    public async Task<IList<OpenApiServer>> GenerateServersAsync(string url, CancellationToken cancellationToken)
    {
        var defaultServer = new OpenApiServer
        {
            Url = url
        };

        List<OpenApiServer> servers = [defaultServer];
        if (options.Value.Servers is not null)
        {
            servers.AddRange(options.Value.Servers);
        }

        foreach (var server in servers)
        {
            var serverContext = new ServerContext(server, serviceProvider, cancellationToken);
            foreach (var serverTransformer in options.Value.GeneratorOptions.ServerTransformers)
            {
                var task = serverTransformer.TransformAsync(serverContext);
                if (!task.IsCompleted)
                {
                    await task;
                }
            }
        }

        return servers;
    }
}