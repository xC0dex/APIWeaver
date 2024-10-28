using Microsoft.Extensions.DependencyInjection;

namespace APIWeaver.Demo.Shared;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddDemoServices(this IServiceCollection services)
    {
        services.AddSingleton<BookStore>();

        services.AddApiWeaver("v1", options =>
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
        
        services.AddOpenApi(options =>
        {
            options.AddResponseDescriptions();
            options.AddOperationTransformer<AdditionalDescriptionTransformer>();
            options.AddDocumentTransformer((document, _) => document.Info.Title = "Book Store API");
        });
        return services;
    }
}