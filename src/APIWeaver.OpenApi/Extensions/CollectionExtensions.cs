namespace APIWeaver;

internal static class CollectionExtensions
{
    /// <summary>
    /// Determines whether any element of an array satisfies a condition.
    /// </summary>
    /// <typeparam name="T">The type of the elements of the array.</typeparam>
    /// <param name="array">The array to apply the predicate to.</param>
    /// <param name="predicate">The predicate to apply to each element.</param>
    /// <returns>true if any elements match the predicate; otherwise, false.</returns>
    internal static bool Any<T>(this T[] array, Predicate<T> predicate) => Array.Exists(array, predicate);

    /// <summary>
    /// Determines if an array has any elements.
    /// </summary>
    /// <typeparam name="T">The type of the elements of the array.</typeparam>
    /// <param name="array">The array to check.</param>
    /// <returns>true if the array has any elements; otherwise, false.</returns>
    internal static bool Any<T>(this T[] array) => array.Length > 0;
}