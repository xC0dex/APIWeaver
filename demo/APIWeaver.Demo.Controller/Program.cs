using APIWeaver;
using APIWeaver.Demo.Shared;
using Asp.Versioning;
using Scalar.AspNetCore;
using static APIWeaver.BuildHelper;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();

builder.Services.AddApiVersioning(options =>
    {
        options.DefaultApiVersion = new ApiVersion(1, 0);
        options.AssumeDefaultVersionWhenUnspecified = true;
    })
    .AddApiExplorer(options =>
    {
        options.GroupNameFormat = "'v'VVV";
        options.SubstituteApiVersionInUrl = true;
    });

builder.Services.AddSingleton<BookStore>();

builder.Services.AddApiWeaver("v1", options =>
{
    options.AddExample(new Book
    {
        BookId = Guid.NewGuid(),
        Title = "Hola",
        Description = "A book about nothing",
        BookType = BookType.Newsletter,
        Pages = 187
    });
});
builder.Services.AddOpenApi(options =>
{
    options.AddOperationTransformer<AdditionalDescriptionTransformer>();
    options.AddDocumentTransformer((document, _) => document.Info.Title = "Book Store API");
});

var app = builder.Build();

// If the current invocation is for document generation, run the application and exit.
if (IsGenerationContext)
{
    app.Run();
}

app.MapOpenApi();
app.MapScalarApiReference();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.Run();