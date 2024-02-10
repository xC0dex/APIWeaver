namespace APIWeaver.OpenApi.MinimalApi;

public class User
{
    public required Guid Id { get; set; }

    public required string Name { get; set; }

    public int? Age { get; set; }

    public IEnumerable<Book>? Books { get; set; }
}

public class Book
{
    public required string Id { get; set; }

    public required string Name { get; set; }
}