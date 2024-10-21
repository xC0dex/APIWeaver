# Getting started with APIWeaver.OpenApi

APIWeaver is a powerful and lightweight library built on top of Microsoft's OpenAPI document generation for .NET 9. It provides extension methods and transformers to easily customize your OpenAPI documents. It's designed to be user-friendly, fully configurable, and AOT compatible.

## Installation

To get started with APIWeaver, you can install the NuGet package `APIWeaver.OpenApi`.

```shell
dotnet add package APIWeaver.OpenApi
```

Add the following using directive to your `Program.cs` file.

```csharp
using APIWeaver;
```

The `APIWeaver.OpenApi` package provides useful extension methods and transformers for OpenAPI documents and operations.

### Authentication

The `APIWeaver.OpenApi` package provides extension methods to add security schemes to your OpenAPI document. The following snippet shows how to add such a scheme to your OpenAPI document:

```csharp
builder.Services.AddOpenApiDocument(options =>
{
    // Adds a security scheme to the OpenAPI document
    // and update all operations with the security requirement
    options.AddSecurityScheme("Bearer", scheme =>
    {
        scheme.In = ParameterLocation.Header;
        scheme.Type = SecuritySchemeType.OAuth2;
        scheme.Flows = new OpenApiOAuthFlows
        {
            ClientCredentials = new OpenApiOAuthFlow
            {
                TokenUrl = new Uri("https://localhost:5001/oauth2/token")
            }
        };
    });

    // Adds 401 and 403 responses
    options.AddAuthResponse();
});
```

The `AddSecurityScheme` method adds a security scheme to the OpenAPI document and updates all operations with the security requirement. The `AddAuthResponse` method checks if the endpoint requires authentication ands adds the `401 - Unauthorized` response to the related operation. If the endpoint requires authorization (roles, policies, default or fallback policies), the `403 - Forbidden` response is added to the operation.


### Other extensions

If you use the `Microsoft.Extensions.ApiDescription.Server` package with build-time document generation, you may want to use the static `BuildHelper` class, which provides a property that indicates whether the current execution context is the document generator. You can use it as follows:
```csharp
using static APIWeaver.BuildHelper;

...

if (!IsGenerationContext)
{
    builder.Services.AddSingleton<IMyService, MyService>();
}

var app = builder.Build();

if (IsGenerationContext)
{
    app.Run();
}
```

## APIWeaver.Swagger

The `APIWeaver.Swagger` package provides everything you need to integrate Swagger UI into your application. The package contains the following extension methods to register an OpenAPI document:

- `AddOpenApiDocument()`: Adds the OpenAPI document to the service collection by calling `AddOpenApi`.
- `AddOpenApiDocuments()`: Adds multiple OpenAPI documents to the service collection.
- `MapOpenApi()`: Adds the OpenAPI document endpoint to the application.

> [!NOTE]  
> Due to the current implementation of `AddOpenApi()` the registration of OpenAPI documents **must** be done with `AddOpenApiDocument()` or `AddOpenApiDocument()`.
> Otherwise you have to provide the routes to the OpenAPI documents manually.

Here are some examples of how to use these extension methods:

```csharp
// Add a single OpenAPI document
builder.Services.AddOpenApiDocument();

// Add multiple OpenAPI documents
builder.Services.AddOpenApiDocuments(["v1", "v2"]);

// Add a single OpenAPI document with a custom configuration
builder.Services.AddOpenApiDocument(options =>
{
    options.AddDocumentTransformer((document, _) => document.Info.Title = "My API");
});

// Add multiple OpenAPI documents with custom configurations
builder.Services.AddOpenApiDocuments(["v1", "v2"], options =>
{
    // Reuse for all documents
    options.AddDocumentTransformer((document, _) => document.Info.Title = "My API");
});
```


### Configure Swagger UI

The Swagger UI can be configured using the `SwaggerOptions` class. This class has different properties and a fluent API to configure the UI. The UI configuration is based on the official Swagger UI configuration with some additional options. The following snippet shows how to configure the Swagger UI:

```csharp
// Fluent API
app.MapSwaggerUi(options =>
{
    options
        .AddStylesheet("https://localhost:5001/custom.css")
        .WithTryItOut(true)
        .WithDarkMode(true);
});

// Object initializer
app.MapSwaggerUi(options =>
{
    options.AddStylesheet("https://localhost:5001/custom.css");
    options.TryItOutEnabled = true;
    options.DarkMode = true;
});
```

There are many more options available to configure the Swagger UI. You can find a list of all available options in the `SwaggerOptions` and `SwaggerUiOptions` class.

### More examples

Require authentication for the Swagger UI:
```csharp
app.MapSwaggerUi().RequireAuthorization();
```

More is coming soon...
