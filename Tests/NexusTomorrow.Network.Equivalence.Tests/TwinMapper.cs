using System.Collections;
using System.Globalization;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;

namespace NexusTomorrow.Network.Equivalence.Tests;

/// <summary>
/// Copies a declared instance onto a new legacy instance by member name, recursively, so the legacy writer is handed the same values.
/// Run it after the generated Write, which assigns the count and selector members a legacy twin may carry.
/// </summary>
internal sealed class TwinMapper
{
    private readonly Func<Type, Type?> _twinOf;
    private readonly IServiceProvider _legacyServices;
    private readonly GraphComparer _comparer;

    public TwinMapper(Func<Type, Type?> twinOf, IServiceProvider legacyServices)
    {
        _twinOf = twinOf;
        _legacyServices = legacyServices;
        _comparer = new GraphComparer(twinOf);
    }

    /// <summary>
    /// A new <paramref name="legacyType"/> holding the wire values of <paramref name="declared"/>; each value that cannot be carried over is added to <paramref name="differences"/>.
    /// </summary>
    public object Map(object declared, Type legacyType, List<string> differences)
    {
        return MapObject(declared, legacyType, declared.GetType().Name, differences);
    }

    private object MapObject(object declared, Type legacyType, string path, List<string> differences)
    {
        object legacy = ActivatorUtilities.CreateInstance(_legacyServices, legacyType);
        foreach (WireMember member in WireMembers.Of(declared.GetType()))
        {
            if (member.Shape == WireShape.Pad)
                continue;

            MemberInfo? twin = MemberAccess.Find(legacyType, member.Name);
            if (twin is null)
            {
                if (!member.IsCount)
                    differences.Add($"{member.QualifiedName} has no legacy twin");
                continue;
            }

            string memberPath = $"{path}.{member.Name}";
            object? value = MemberAccess.GetValue(member.Member, declared);
            Type twinType = MemberAccess.TypeOf(twin);
            if (member.Shape is WireShape.Array or WireShape.FixedArray && value is IEnumerable items)
            {
                Type elementType = CollectionAccess.ElementTypeOf(twinType);
                object?[] elements = [.. items.Cast<object?>().Select((item, index) => MapElement(member.Encoding, item, elementType, $"{memberPath}[{index}]", differences))];

                // A get-only legacy list is refilled in place; any other member gets a new collection of its own type.
                if (MemberAccess.HasSetter(twin) || !CollectionAccess.TryRefill(MemberAccess.GetValue(twin, legacy), elements))
                    Assign(member, value, twin, legacy, CollectionAccess.Create(twinType, elementType, elements), memberPath, differences);
                continue;
            }

            Assign(member, value, twin, legacy, MapValue(member, value, twinType, memberPath, differences), memberPath, differences);
        }

        return legacy;
    }

    // A legacy member with neither a setter nor a backing field is computed, so it carries the declared value only if it already computes it.
    private void Assign(WireMember member, object? declaredValue, MemberInfo twin, object legacy, object? mapped, string path, List<string> differences)
    {
        if (!MemberAccess.TrySetValue(twin, legacy, mapped))
            _comparer.CompareMember(member, declaredValue, MemberAccess.GetValue(twin, legacy), path, differences);
    }

    private object? MapValue(WireMember member, object? value, Type targetType, string path, List<string> differences)
    {
        if (value is null)
            return null;

        return member.Shape switch
        {
            WireShape.Union => MapTwin(value, null, path, differences),
            WireShape.Blob => ((byte[])value).Clone(),
            _ => MapElement(member.Encoding, value, targetType, path, differences),
        };
    }

    private object? MapElement(WireEncoding encoding, object? value, Type targetType, string path, List<string> differences)
    {
        if (value is null)
            return null;

        Type target = Nullable.GetUnderlyingType(targetType) ?? targetType;
        if (encoding == WireEncoding.Struct)
            return MapTwin(value, target, path, differences);
        if (ScalarValues.TryGetInteger(value, out Int128 number) && ScalarValues.IsInteger(target))
            return ScalarValues.FromInteger(number, target);

        return target.IsInstanceOfType(value) ? value : Convert.ChangeType(value, target, CultureInfo.InvariantCulture);
    }

    /// <summary>
    /// Map a nested value onto its twin type, or onto <paramref name="fallback"/> (the legacy member's own type) when the resolver knows no twin.
    /// A union arm has no fallback: the resolver must name its twin.
    /// </summary>
    private object? MapTwin(object value, Type? fallback, string path, List<string> differences)
    {
        Type? twin = _twinOf(value.GetType()) ?? (fallback is { IsClass: true, IsAbstract: false } ? fallback : null);
        if (twin is null)
        {
            differences.Add($"{path}: {value.GetType().Name} has no legacy twin");
            return null;
        }

        return MapObject(value, twin, path, differences);
    }
}
