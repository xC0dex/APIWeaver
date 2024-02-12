# Getting started with APIWeaver

## APIWeaver configuration

API Weaver is highly configurable and can be customized to fit your needs by using the `OpenApiOptions` class. 

### Transformers

Transformers are a way to modify the generated OpenAPI segments like the document, operations, servers or schemes. There are three different ways how transformers can be added to the options:

```csharp
builder.Services.AddApiWeaver(options =>
{
    options.WithGeneratorOptions(generatorOptions =>
    {
        // 1. Add a transformer using a async function
        generatorOptions.OperationTransformers.Add(async context =>
        {
            var customService = context.ServiceProvider.GetRequiredService<ICustomService>();
            var operationId = await customService.GetOperationIdAsync(context.OpenApiOperation, context.CancellationToken);
            context.OpenApiOperation.OperationId = operationId;
        });

        // 2. Add a transformer using a sync function
        generatorOptions.DocumentTransformers.Add(context =>
        {
            context.OpenApiDocument.Info.Description = "Some additional description";
        });
        
        // 3. Add a transformer using a implementation of IServerTransformer
        generatorOptions.ServerTransformers.Add(new CustomServerTransformer());
    });
});
```

More is coming soon...

## Swagger UI configuration

The Swagger UI can be configured using the `SwaggerOptions` class. This class has different properties and a fluent API to configure the UI. The UI configuration is based on the official [Swagger UI configuration](https://github.com/swagger-api/swagger-ui/blob/master/docs/usage/configuration.md). The following snippet shows how to configure the Swagger UI:

```csharp
app.UseSwaggerUi(options =>
{
    options.Title = "Swagger UI";
    options.WithUiOptions(swaggerUiOptions =>
    {
        swaggerUiOptions.TryItOutEnabled = true;
        swaggerUiOptions.DisplayOperationId = true;
    });
    options.WithOAuth2Options(oAuth2Options => oAuth2Options.ClientSecret = "my-client-secret");
});
```

More is coming soon...