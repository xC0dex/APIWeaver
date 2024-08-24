# APIWeaver

[![Pipeline](https://github.com/xC0dex/APIWeaver/actions/workflows/ci.yml/badge.svg)](https://github.com/xC0dex/APIWeaver/actions/workflows/ci.yml)
[![Coverage](https://sonarcloud.io/api/project_badges/measure?project=xC0dex_APIWeaver&metric=coverage)](https://sonarcloud.io/summary/new_code?id=xC0dex_APIWeaver)
[![NuGet Version](https://img.shields.io/nuget/v/APIWeaver.Swagger)](https://www.nuget.org/packages/APIWeaver.Swagger/)
[![NuGet Downloads](https://img.shields.io/nuget/dt/APIWeaver.Swagger)](https://www.nuget.org/packages/APIWeaver.Swagger/)

APIWeaver is a powerful and lightweight library designed to provide a seamless integration Swagger UI into your .NET 9+ apps. APIWeaver supports the latest .NET features and is designed to be future-proof, ensuring support for upcoming .NET and OpenAPI features.

## Getting Started

To get started with APIWeaver, you can install the NuGet package using your preferred package manager. In most cases, the package `APIWeaver.Swagger` is the one you are looking for.

1. Install the NuGet package

```shell
dotnet add package APIWeaver.Swagger
```

2. Add the using directive to your Program.cs file

```csharp
using APIWeaver;
```

3. Add the following lines to your `Program.cs` file.

```csharp
builder.Services.AddOpenApiDocument();
// other code

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapSwaggerUi();
}
```

That's it. You now have a fully functional Swagger UI in your application. The UI can be accessed by navigating to `/swagger` in your browser 🥳. 
## Currently supported features

- Fully functional and configurable Swagger UI
- Dark mode 🌙
- Useful extension methods

## Roadmap

- Useful transformers for OpenAPI documents and operations. Like, e.g, For Security schemes.
- Support for more OpenAPI UIs
- If I have enough time: API client generation based on the generated OpenAPI document

## Work in progress

This project is currently under **active development**, with ongoing improvements, new features and potential API changes. If you have any feedback, feature requests or issues, please feel free to open an issue or a pull request.


## Contribution and Collaboration

Your contributions to this project are welcomed and encouraged. Your active involvement can significantly impact its success!

## License

Distributed under the MIT License. See [LICENSE](LICENSE) for more information.
This means APIWeaver will always remain free.
