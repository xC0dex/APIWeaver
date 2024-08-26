# Getting started with APIWeaver

`ApiWeaver` is a library that provides a fully functional and configurable Swagger UI for your ASP.NET Core application.

## Installation

To get started with APIWeaver, you can install the NuGet package `APIWeaver`.

```shell
dotnet add package APIWeaver
```

The `APIWeaver` package bundles the `APIWeaver.Swagger` and `APIWeaver.OpenApi` packages. The `APIWeaver.Swagger` package provides everything you need to integrate Swagger UI into your application, while the `APIWeaver.OpenApi` package provides useful extension methods and transformers for OpenAPI documents and operations.

## Basic usage

Add the following using directive to your `Program.cs` file.

```csharp
using APIWeaver;
```

Add the following lines to your `Program.cs` file.

```csharp
builder.Services.AddOpenApiDocument();

// other code

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapSwaggerUi();
}
```

This is the minimum configuration required to get started with APIWeaver. You now have a fully functional Swagger UI in your application. The UI can be accessed by navigating to `/swagger` in your browser 🥳.

## Configure Swagger UI

The Swagger UI can be configured using the `SwaggerOptions` class. This class has different properties and a fluent API to configure the UI. The UI configuration is based on the official Swagger UI configuration with some additional options. The following snippet shows how to configure the Swagger UI:

```csharp
app.MapSwaggerUi(options =>
{
    options
        .AddStylesheet("https://localhost:5001/custom.css")
        .WithTryItOut(true)
        .WithDarkMode(true);
});
```









<!-- ## APIWeaver configuration

API Weaver is highly configurable and can be customized to fit your needs by using the `OpenApiOptions` class.  -->


<!-- ### Transformers

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
``` -->

More is coming soon...