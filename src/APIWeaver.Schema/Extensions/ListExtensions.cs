namespace APIWeaver.Schema.Extensions;

internal static class ListExtensions
{
    public static void AddWhen<T>(this IList<T> list, bool condition, T item)
    {
        if (condition)
        {
            list.Add(item);
        }
    }

    public static void Apply<T>(this IEnumerable<Action<T>> list, T item)
    {
        foreach (var action in list)
        {
            action.Invoke(item);
        }
    }
}