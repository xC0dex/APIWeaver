# APIWeaver

[![Pipeline](https://github.com/xC0dex/APIWeaver/actions/workflows/ci.yml/badge.svg)](https://github.com/xC0dex/APIWeaver/actions/workflows/ci.yml)
[![Coverage](https://sonarcloud.io/api/project_badges/measure?project=xC0dex_APIWeaver&metric=coverage)](https://sonarcloud.io/summary/new_code?id=xC0dex_APIWeaver)
[![NuGet Version](https://img.shields.io/nuget/v/APIWeaver.Swagger)](https://www.nuget.org/packages/APIWeaver.Swagger/)
[![NuGet Version](https://img.shields.io/nuget/dt/APIWeaver.OpenApi)](https://www.nuget.org/packages/APIWeaver.Swagger/)


APIWeaver is a powerful, lightweight, and feature-rich library designed to provide comprehensive OpenAPI support in .NET
6 and newer.

APIWeaver is committed to long-term development, ensuring support for upcoming .NET and OpenAPI features.

## Work in progress

This project is currently in the **active development** phase, with ongoing improvements, new features and potential API
changes. Contributions of any kind are highly appreciated!

## Getting Started
To get started with APIWeaver, you can install the NuGet package using your preferred package manager. In most cases, the package `APIWeaver.Swagger` is the one you are looking for.

1. Install the NuGet package
```shell
dotnet add package APIWeaver.Swagger
```

2. Add the using directive to your Program.cs file
```csharp
using APIWeaver.Swagger;
```

3. Add the following line to your `Program.cs` file
```csharp
builder.Services.AddApiWeaver();

// other code

if (app.Environment.IsDevelopment())
{
    app.UseSwaggerUi();
}
```

Thats it. You now have a fully functional Swagger UI in your application. The UI can be accessed by navigating to `/swagger` in your browser 🥳. A more detailed guide with more usecases can be found [here](/docs/Getting-Started.md).


## Contribution and Collaboration

Your contributions to this project are welcomed and encouraged. Your active involvement can significantly impact its
success!

## License

Distributed under the MIT License. See LICENSE for more information.
This means APIWeaver will always remain free.
