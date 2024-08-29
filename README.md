# APIWeaver

[![Pipeline](https://github.com/xC0dex/APIWeaver/actions/workflows/ci.yml/badge.svg)](https://github.com/xC0dex/APIWeaver/actions/workflows/ci.yml)
[![Coverage](https://sonarcloud.io/api/project_badges/measure?project=xC0dex_APIWeaver&metric=coverage)](https://sonarcloud.io/summary/new_code?id=xC0dex_APIWeaver)
[![NuGet Version](https://img.shields.io/nuget/v/APIWeaver.Swagger)](https://www.nuget.org/packages/APIWeaver/)
[![NuGet Downloads](https://img.shields.io/nuget/dt/APIWeaver.Swagger)](https://www.nuget.org/packages/APIWeaver.Swagger/)

APIWeaver is a powerful and lightweight library that integrates Swagger UI and OpenAPI support into your .NET 9 applications. It is designed to be user-friendly, fully configurable, and AOT compatible. Built on top of Microsoft's built-in OpenAPI document generation, the library offers a range of extension methods and transformers to enhance OpenAPI documents and operations.

## Features

- Fully functional and configurable Swagger UI
- Dark mode support 🌙
- Native AOT compatible
- Useful extension methods & transformers for OpenAPI documents and operations
- High test coverage ensures reliability 🔍

## 🚀 Getting Started

Getting up and running with APIWeaver is simple:

1. **Install the package**

```shell
dotnet add package APIWeaver
```

2. **Add the necessary using directive**

```csharp
using APIWeaver;
```

3. **Configure your application**

Add the following lines to your `Program.cs` file to set up the Swagger UI:

```csharp
builder.Services.AddOpenApiDocument();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapSwaggerUi();
}
```

And that's it! 🎉 Your application now has a fully functional, AOT-compatible Swagger UI. Head to `/swagger` in your browser to check it out.
For more advanced configuration options, check out the [documentation](docs/Getting-Started.md).

## 🛠️ Roadmap
- More useful transformers for OpenAPI documents and operations.
- Support for more OpenAPI UIs
- Later: API client generation based on the generated OpenAPI document

## Contribution and Collaboration

Contributions are welcome! Feel free to open issues or pull requests.

## License

This project is licensed under the MIT License. See [LICENSE](LICENSE) for more information.
This means APIWeaver will always remain free.
