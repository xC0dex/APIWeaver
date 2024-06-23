namespace APIWeaver.OpenApi.Providers;

internal class CustomTestProvider
{
    private readonly IServiceProvider serviceProvider;

    public CustomTestProvider(IServiceProvider serviceProvider)
    {
        this.serviceProvider = serviceProvider;
    }

    public async Task<string> Provide(string id)
    {
        return await Task.FromResult(id);
    }
}