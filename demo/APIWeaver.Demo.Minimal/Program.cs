using APIWeaver;
using APIWeaver.Demo.Minimal;
using APIWeaver.Demo.Shared;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddSingleton<BookStore>();

builder.Services.AddApiWeaver("v1", options =>
{
    options
        .AddExample(new Book
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

app.UseDeveloperExceptionPage();
app.MapOpenApi();
app.MapScalarApiReference(o =>
{
    o.WithTheme(ScalarTheme.Mars);
});

app.MapBookEndpoints();

app.Run();