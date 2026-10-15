using System.Collections;
using System.Globalization;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;

namespace NexusTomorrow.Network.Equivalence.Tests;

/// <summary>
/// Copies a declared instance onto a new legacy instance by member name, recursively, so the legacy writer is handed the same values; a declared byte[] fills a legacy bit array's buffer.
/// A union member with no legacy twin lands on the legacy holder itself: its arm in the legacy member kept for that arm, or its arm's members on the holder's flat members of the same names.
/// The pairing's rows, presets and renames apply where they are pinned.
/// Run it after the generated Write, which assigns the count and selector members a legacy twin may carry.
/// </summary>
internal sealed class TwinMapper
{
    private readonly Pairing _pairing;
    private readonly IServiceProvider _legacyServices;
    private readonly GraphComparer _comparer;

    public TwinMapper(Pairing pairing, IServiceProvider legacyServices)
    {
        _pairing = pairing;
        _legacyServices = legacyServices;
        _comparer = new GraphComparer(pairing);
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
        _pairing.Preset(declared.GetType(), legacy);
        MapMembers(declared, legacy, path, differences);
        return legacy;
    }

    private void MapMembers(object declared, object legacy, string path, List<string> differences)
    {
        Type legacyType = legacy.GetType();
        IReadOnlyList<WireMember> members = WireMembers.Of(declared.GetType());
        RowColumns? rows = _pairing.RowsOf(declared.GetType());
        foreach (WireMember member in members)
        {
            if (member.Shape == WireShape.Pad || rows?.Holds(member.Name) == true)
                continue;

            string memberPath = $"{path}.{member.Name}";
            object? value = MemberAccess.GetValue(member.Member, declared);
            MemberInfo? twin = MemberAccess.Find(legacyType, _pairing.LegacyNameOf(member));
            if (twin is null)
            {
                if (member.Shape == WireShape.Union)
                {
                    if (value is not null)
                        MapArm(value, legacy, member.Name, memberPath, differences);
                }
                else if (!member.IsCount && !WireMembers.IsSelector(member, members))
                {
                    differences.Add($"{member.QualifiedName} has no legacy twin");
                }

                continue;
            }

            Type twinType = MemberAccess.TypeOf(twin);
            if (twinType == typeof(NetworkBitArray) && value is byte[] bytes)
            {
                if (BitArrayOf(bytes, memberPath, differences) is { } bits)
                    Assign(member, value, twin, legacy, bits, memberPath, differences);
                continue;
            }

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

        if (rows is not null)
            MapRows(declared, members, legacy, rows, path, differences);
    }

    /// <summary>
    /// Put the arm of a union the legacy holder has no member for onto the holder: into the member the holder keeps for that arm when the arm has a twin,
    /// else member by member onto the holder's own members.
    /// </summary>
    private void MapArm(object arm, object legacy, string unionName, string path, List<string> differences)
    {
        if (_pairing.TwinOf(arm.GetType()) is not { } armTwin)
        {
            MapMembers(arm, legacy, path, differences);
            return;
        }

        if (Pairing.ArmSlotOf(legacy.GetType(), unionName, armTwin) is { } slot)
            MemberAccess.SetValue(slot, legacy, MapObject(arm, armTwin, path, differences));
        else
            differences.Add($"{path}: {legacy.GetType().Name} has no member for arm {arm.GetType().Name}");
    }

    /// <summary>
    /// Element i of each column goes into its field of record i of the legacy list, adding records the list does not have yet.
    /// A record that is a struct is read out boxed, set, and stored back, since the list hands out copies.
    /// </summary>
    private void MapRows(object declared, IReadOnlyList<WireMember> members, object legacy, RowColumns rows, string path, List<string> differences)
    {
        MemberInfo? list = MemberAccess.Find(legacy.GetType(), rows.LegacyList);
        if (list is null || MemberAccess.GetValue(list, legacy) is not IList { IsFixedSize: false } records)
        {
            differences.Add($"{path}: {legacy.GetType().Name} has no growable list {rows.LegacyList}");
            return;
        }

        Type recordType = CollectionAccess.ElementTypeOf(MemberAccess.TypeOf(list));
        foreach ((string column, string field) in rows.Columns)
        {
            WireMember member = members.Single(candidate => candidate.Name == column);
            object?[] values = MemberAccess.GetValue(member.Member, declared) is IEnumerable items ? [.. items.Cast<object?>()] : [];
            if (MemberAccess.Find(recordType, field) is not { } target)
            {
                differences.Add($"{path}.{column}: {recordType.Name} has no field {field}");
                continue;
            }

            if (records.Count > values.Length)
                differences.Add($"{path}.{column}: {values.Length} values for {records.Count} records");
            while (records.Count < values.Length)
                records.Add(recordType.IsValueType ? Activator.CreateInstance(recordType) : ActivatorUtilities.CreateInstance(_legacyServices, recordType));

            for (int i = 0; i < values.Length; i++)
            {
                object record = records[i]!;
                MemberAccess.SetValue(target, record, MapElement(member.Encoding, values[i], MemberAccess.TypeOf(target), $"{path}.{column}[{i}]", differences));
                records[i] = record;
            }
        }
    }

    /// <summary>
    /// A legacy bit array whose buffer holds <paramref name="bytes"/>: a declared byte[] stands for one, as the legacy writer sends a bit array's buffer byte by byte.
    /// Null, with a difference, for no bytes, since a bit array holds at least one.
    /// </summary>
    private static NetworkBitArray? BitArrayOf(byte[] bytes, string path, List<string> differences)
    {
        if (bytes.Length == 0)
        {
            differences.Add($"{path}: a NetworkBitArray cannot hold zero bytes");
            return null;
        }

        var bits = new NetworkBitArray((uint)bytes.Length * 8, NetworkBitArray.BitOrder.LeastSignificantBit);
        bytes.CopyTo(bits.GetBuffer(), 0);
        return bits;
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
        {
            // A declared holder of one union maps onto the bare arm where the legacy model keeps the arm itself and computes its selector.
            if (WireMembers.SingleUnionOf(value.GetType()) is { } union && (_pairing.TwinOf(value.GetType()) is not { } holderTwin || !target.IsAssignableFrom(holderTwin)))
            {
                object? arm = MemberAccess.GetValue(union.Member, value);
                return arm is null ? null : MapTwin(arm, null, $"{path}.{union.Name}", differences);
            }

            return MapTwin(value, target, path, differences);
        }
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
        Type? twin = _pairing.TwinOf(value.GetType()) ?? (fallback is { IsClass: true, IsAbstract: false } ? fallback : null);
        if (twin is null)
        {
            differences.Add($"{path}: {value.GetType().Name} has no legacy twin");
            return null;
        }

        return MapObject(value, twin, path, differences);
    }
}
