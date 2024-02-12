# APIWeaver

[![Pipeline](https://github.com/xC0dex/APIWeaver/actions/workflows/ci.yml/badge.svg)](https://github.com/xC0dex/APIWeaver/actions/workflows/ci.yml)
[![Coverage](https://sonarcloud.io/api/project_badges/measure?project=xC0dex_APIWeaver&metric=coverage)](https://sonarcloud.io/summary/new_code?id=xC0dex_APIWeaver)
[![NuGet Version](https://img.shields.io/nuget/v/APIWeaver.Swagger)](https://www.nuget.org/packages/APIWeaver.Swagger/)
[![NuGet Downloads](https://img.shields.io/nuget/dt/APIWeaver.OpenApi)](https://www.nuget.org/packages/APIWeaver.Swagger/)


APIWeaver is a powerful and lightweight library designed to provide a seamless integration of OpenAPI and Swagger UI into your .NET 6+ apps. APIWeaver supports the latest .NET features and is designed to be future-proof, ensuring support for upcoming .NET and OpenAPI features.

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
builder.Services.AddApiWeaver();

// other code

if (app.Environment.IsDevelopment())
{
    app.UseSwaggerUi();
}
```

That's it. You now have a fully functional Swagger UI in your application. The UI can be accessed by navigating to `/swagger` in your browser 🥳. A more detailed guide with more use cases can be found [here](/docs/Getting-Started.md).

## Currently supported features

- Fully functional and configurable Swagger UI (with dark mode 🌙)
- OpenAPI 3.0 generation
- Document generation based on minimal APIs
- .NET 6, 7 and 8 including required members, nullable types and latest validation attributes

## Roadmap

- Support for controller based APIs (https://github.com/xC0dex/APIWeaver/issues/27)
- Full `System.Text.Json` support ([Milestone](https://github.com/xC0dex/APIWeaver/milestone/1))
- Security definitions for OpenAPI documents
- API client generation based on the generated OpenAPI document
- ... and many more

## Work in progress

This project is currently under **active development**, with ongoing improvements, new features and potential API changes. If you have any feedback, feature requests or issues, please feel free to open an issue or a pull request.


## Contribution and Collaboration

Your contributions to this project are welcomed and encouraged. Your active involvement can significantly impact its success!

## License

Distributed under the MIT License. See [LICENSE](LICENSE) for more information.
This means APIWeaver will always remain free.
