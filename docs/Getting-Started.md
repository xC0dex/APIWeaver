# Getting started with APIWeaver

## APIWeaver configuration

API Weaver is highly configurable and can be customized to fit your needs.

### Transformers

Transformers are a way to modify the generated OpenAPI segments like the document, operations or servers. There are three different ways how transformers can be added to the options:
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
        
        // 3. Add a transformer using a class
        generatorOptions.ServerTransformers.Add(new CustomServerTransformer());
    });
});
```

## Swagger UI configuration

