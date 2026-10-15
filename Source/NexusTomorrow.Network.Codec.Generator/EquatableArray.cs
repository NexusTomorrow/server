using System;
using System.Collections;
using System.Collections.Generic;

namespace NexusTomorrow.Network.Codec.Generator;

/// <summary>
/// An array compared by its elements, so a model holding one caches between generator runs.
/// </summary>
internal readonly struct EquatableArray<T> : IEquatable<EquatableArray<T>>, IEnumerable<T>
    where T : IEquatable<T>
{
    private readonly T[]? _items;

    public EquatableArray(T[] items)
    {
        _items = items;
    }

    public static EquatableArray<T> Empty { get; } = new([]);

    public int Count => _items?.Length ?? 0;

    public T this[int index] => (_items ?? [])[index];

    public static bool operator ==(EquatableArray<T> left, EquatableArray<T> right)
    {
        return left.Equals(right);
    }

    public static bool operator !=(EquatableArray<T> left, EquatableArray<T> right)
    {
        return !left.Equals(right);
    }

    public bool Equals(EquatableArray<T> other)
    {
        T[] mine = _items ?? [];
        T[] theirs = other._items ?? [];
        if (mine.Length != theirs.Length)
            return false;

        for (int i = 0; i < mine.Length; i++)
        {
            if (!mine[i].Equals(theirs[i]))
                return false;
        }

        return true;
    }

    public override bool Equals(object? obj)
    {
        return obj is EquatableArray<T> other && Equals(other);
    }

    public override int GetHashCode()
    {
        int hash = 17;
        foreach (T item in _items ?? [])
            hash = unchecked((hash * 31) + item.GetHashCode());
        return hash;
    }

    public IEnumerator<T> GetEnumerator()
    {
        return ((IEnumerable<T>)(_items ?? [])).GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}
