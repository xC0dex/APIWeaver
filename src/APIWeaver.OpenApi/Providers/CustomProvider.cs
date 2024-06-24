namespace APIWeaver.OpenApi.Providers;

internal sealed class CustomProvider
{
    private const string Fallback = "fallback";

    public static string Provide(bool isTrue, string result)
    {
        return isTrue ? result : Fallback;
    }
}