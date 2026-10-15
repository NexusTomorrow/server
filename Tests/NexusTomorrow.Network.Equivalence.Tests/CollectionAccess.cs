using System.Collections;

namespace NexusTomorrow.Network.Equivalence.Tests;

/// <summary>
/// Builds and refills list and array members through reflection.
/// </summary>
internal static class CollectionAccess
{
    public static Type ElementTypeOf(Type collection)
    {
        if (collection.IsArray)
            return collection.GetElementType()!;

        Type? enumerable = collection.IsGenericType && collection.GetGenericTypeDefinition() == typeof(IEnumerable<>)
            ? collection
            : Array.Find(collection.GetInterfaces(), candidate => candidate.IsGenericType && candidate.GetGenericTypeDefinition() == typeof(IEnumerable<>));
        return enumerable?.GetGenericArguments()[0] ?? throw new InvalidOperationException($"{collection.Name} is not a collection.");
    }

    /// <summary>
    /// A new <paramref name="collectionType"/> holding <paramref name="elements"/>; an interface or abstract collection type gets a <see cref="List{T}"/>.
    /// </summary>
    public static object Create(Type collectionType, Type elementType, object?[] elements)
    {
        if (collectionType.IsArray)
        {
            var array = Array.CreateInstance(elementType, elements.Length);
            for (int i = 0; i < elements.Length; i++)
                array.SetValue(elements[i], i);
            return array;
        }

        Type concrete = collectionType.IsInterface || collectionType.IsAbstract ? typeof(List<>).MakeGenericType(elementType) : collectionType;
        var list = (IList)Activator.CreateInstance(concrete)!;
        foreach (object? element in elements)
            list.Add(element);
        return list;
    }

    /// <summary>
    /// Replace the contents of <paramref name="existing"/> with <paramref name="elements"/>: a list is cleared and refilled, an array of the same length is overwritten.
    /// False when <paramref name="existing"/> is not a list or is an array of another length.
    /// </summary>
    public static bool TryRefill(object? existing, object?[] elements)
    {
        if (existing is not IList list)
            return false;

        if (list.IsFixedSize)
        {
            if (list.Count != elements.Length)
                return false;

            for (int i = 0; i < elements.Length; i++)
                list[i] = elements[i];
            return true;
        }

        list.Clear();
        foreach (object? element in elements)
            list.Add(element);
        return true;
    }
}
