using System.Collections;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Numerics;
using System.Reflection;

namespace NexusTomorrow.Network.Equivalence.Tests;

/// <summary>
/// Compares a declared instance with a legacy instance member by member, with the twin mapper's name mapping, skips, union flattening, rows and renames.
/// Numbers compare by value, floats and doubles by bit pattern, strings by ordinal, collections by count then element, a legacy bit array by its buffer's bytes, and union arms must be twins.
/// </summary>
internal sealed class GraphComparer
{
    private const int MaxTextLength = 40;

    private readonly Pairing _pairing;

    public GraphComparer(Pairing pairing)
    {
        _pairing = pairing;
    }

    /// <summary>
    /// Every difference between <paramref name="declared"/> and <paramref name="legacy"/>, as a path from the declared type's name: "Type.Member[index].Member: declared != legacy".
    /// </summary>
    public List<string> Compare(object declared, object legacy)
    {
        var differences = new List<string>();
        CompareObject(declared, legacy, declared.GetType().Name, differences);
        return differences;
    }

    /// <summary>
    /// Compare one member's declared and legacy values, adding each difference under <paramref name="path"/>.
    /// </summary>
    public void CompareMember(WireMember member, object? declared, object? legacy, string path, List<string> differences)
    {
        // A declared byte[] stands for a legacy bit array, so the two compare by the bit array's buffer.
        if (legacy is NetworkBitArray bits)
            legacy = bits.GetBuffer();

        switch (member.Shape)
        {
            case WireShape.Value:
                CompareValues(member.Encoding, declared, legacy, path, differences);
                break;
            case WireShape.Array or WireShape.FixedArray:
                CompareCollections(member.Encoding, declared, legacy, path, differences);
                break;
            case WireShape.Union:
                CompareArms(declared, legacy, path, differences);
                break;
            case WireShape.Blob:
                CompareBlobs(declared, legacy, path, differences);
                break;
        }
    }

    private void CompareObject(object declared, object legacy, string path, List<string> differences)
    {
        Type legacyType = legacy.GetType();
        IReadOnlyList<WireMember> members = WireMembers.Of(declared.GetType());
        RowColumns? rows = _pairing.RowsOf(declared.GetType());
        foreach (WireMember member in members)
        {
            if (member.Shape == WireShape.Pad || rows?.Holds(member.Name) == true)
                continue;

            string memberPath = $"{path}.{member.Name}";
            MemberInfo? twin = MemberAccess.Find(legacyType, _pairing.LegacyNameOf(member));
            if (twin is null)
            {
                if (member.Shape == WireShape.Union)
                    CompareArmOnHolder(MemberAccess.GetValue(member.Member, declared), legacy, member.Name, memberPath, differences);
                else if (!member.IsCount && !WireMembers.IsSelector(member, members))
                    differences.Add($"{member.QualifiedName} has no legacy twin");
                continue;
            }

            CompareMember(member, MemberAccess.GetValue(member.Member, declared), MemberAccess.GetValue(twin, legacy), memberPath, differences);
        }

        if (rows is not null)
            CompareRows(declared, members, legacy, rows, path, differences);
    }

    /// <summary>
    /// Compare the arm of a union the legacy holder has no member for with the holder, as the twin mapper puts it there: with the member the holder keeps for
    /// that arm when the arm has a twin, else member by member with the holder's own members.
    /// </summary>
    private void CompareArmOnHolder(object? arm, object legacy, string unionName, string path, List<string> differences)
    {
        if (arm is null)
        {
            differences.Add($"{path}: null != the arm {legacy.GetType().Name} holds");
            return;
        }

        if (_pairing.TwinOf(arm.GetType()) is not { } armTwin)
        {
            CompareObject(arm, legacy, path, differences);
            return;
        }

        if (Pairing.ArmSlotOf(legacy.GetType(), unionName, armTwin) is { } slot)
            CompareArms(arm, MemberAccess.GetValue(slot, legacy), path, differences);
        else
            differences.Add($"{path}: {legacy.GetType().Name} has no member for arm {arm.GetType().Name}");
    }

    /// <summary>
    /// Compare element i of each column with its field of record i of the legacy list.
    /// </summary>
    private void CompareRows(object declared, IReadOnlyList<WireMember> members, object legacy, RowColumns rows, string path, List<string> differences)
    {
        MemberInfo? list = MemberAccess.Find(legacy.GetType(), rows.LegacyList);
        object?[] records = list is not null && MemberAccess.GetValue(list, legacy) is IEnumerable items ? [.. items.Cast<object?>()] : [];
        foreach ((string column, string field) in rows.Columns)
        {
            WireMember member = members.Single(candidate => candidate.Name == column);
            object?[] values = MemberAccess.GetValue(member.Member, declared) is IEnumerable columnItems ? [.. columnItems.Cast<object?>()] : [];
            string columnPath = $"{path}.{column}";
            if (values.Length != records.Length)
            {
                differences.Add($"{columnPath}.Count: {values.Length} != {records.Length} records in {rows.LegacyList}");
                continue;
            }

            for (int i = 0; i < values.Length; i++)
            {
                object? record = records[i];
                MemberInfo? target = record is null ? null : MemberAccess.Find(record.GetType(), field);
                if (record is null || target is null)
                    differences.Add($"{columnPath}[{i}]: record {i} of {rows.LegacyList} has no field {field}");
                else
                    CompareValues(member.Encoding, values[i], MemberAccess.GetValue(target, record), $"{columnPath}[{i}]", differences);
            }
        }
    }

    private void CompareValues(WireEncoding encoding, object? declared, object? legacy, string path, List<string> differences)
    {
        if (!BothPresent(declared, legacy, path, differences))
            return;

        if (encoding == WireEncoding.Struct)
        {
            // A declared holder of one union compares through to the bare arm the legacy model keeps.
            if (WireMembers.SingleUnionOf(declared.GetType()) is { } union && _pairing.TwinOf(declared.GetType()) != legacy.GetType())
            {
                CompareArms(MemberAccess.GetValue(union.Member, declared), legacy, $"{path}.{union.Name}", differences);
                return;
            }

            CompareObject(declared, legacy, path, differences);
            return;
        }

        bool same = encoding switch
        {
            WireEncoding.UInt or WireEncoding.SInt => ScalarValues.TryGetInteger(declared, out Int128 left) && ScalarValues.TryGetInteger(legacy, out Int128 right) && left == right,
            WireEncoding.WideString or WireEncoding.NarrowString or WireEncoding.TerminatedWideString => declared is string left && legacy is string right && string.Equals(left, right, StringComparison.Ordinal),
            _ => SameBits(declared, legacy),
        };
        if (!same)
            differences.Add(Mismatch(path, declared, legacy));
    }

    private void CompareCollections(WireEncoding encoding, object? declared, object? legacy, string path, List<string> differences)
    {
        if (!BothPresent(declared, legacy, path, differences))
            return;

        if (declared is not IEnumerable declaredItems || legacy is not IEnumerable legacyItems)
        {
            differences.Add(Mismatch(path, declared, legacy));
            return;
        }

        object?[] left = [.. declaredItems.Cast<object?>()];
        object?[] right = [.. legacyItems.Cast<object?>()];
        if (left.Length != right.Length)
        {
            differences.Add($"{path}.Count: {left.Length} != {right.Length}");
            return;
        }

        for (int i = 0; i < left.Length; i++)
            CompareValues(encoding, left[i], right[i], $"{path}[{i}]", differences);
    }

    private void CompareArms(object? declared, object? legacy, string path, List<string> differences)
    {
        if (!BothPresent(declared, legacy, path, differences))
            return;

        Type? twin = _pairing.TwinOf(declared.GetType());
        if (twin is null)
            differences.Add($"{path}: {declared.GetType().Name} has no legacy twin");
        else if (legacy.GetType() != twin)
            differences.Add($"{path}: arm {declared.GetType().Name} != arm {legacy.GetType().Name}");
        else
            CompareObject(declared, legacy, path, differences);
    }

    private static void CompareBlobs(object? declared, object? legacy, string path, List<string> differences)
    {
        if (BothPresent(declared, legacy, path, differences) && (declared is not byte[] left || legacy is not byte[] right || !left.AsSpan().SequenceEqual(right)))
            differences.Add(Mismatch(path, declared, legacy));
    }

    private static bool BothPresent([NotNullWhen(true)] object? declared, [NotNullWhen(true)] object? legacy, string path, List<string> differences)
    {
        if (declared is not null && legacy is not null)
            return true;
        if (declared is not null || legacy is not null)
            differences.Add(Mismatch(path, declared, legacy));
        return false;
    }

    private static bool SameBits(object declared, object legacy)
    {
        return (declared, legacy) switch
        {
            (float left, float right) => BitConverter.SingleToUInt32Bits(left) == BitConverter.SingleToUInt32Bits(right),
            (double left, double right) => BitConverter.DoubleToUInt64Bits(left) == BitConverter.DoubleToUInt64Bits(right),
            (Vector3 left, Vector3 right) => SameBits(left.X, right.X) && SameBits(left.Y, right.Y) && SameBits(left.Z, right.Z),
            (Vector2 left, Vector2 right) => SameBits(left.X, right.X) && SameBits(left.Y, right.Y),
            (Quaternion left, Quaternion right) => SameBits(left.X, right.X) && SameBits(left.Y, right.Y) && SameBits(left.Z, right.Z) && SameBits(left.W, right.W),
            _ => Equals(declared, legacy),
        };
    }

    private static string Mismatch(string path, object? declared, object? legacy)
    {
        return $"{path}: {Describe(declared)} != {Describe(legacy)}";
    }

    private static string Describe(object? value)
    {
        return value switch
        {
            null => "null",
            string text when text.Length > MaxTextLength => $"\"{text[..MaxTextLength]}...\" ({text.Length} units)",
            string text => $"\"{text}\"",
            float single => $"{single.ToString(CultureInfo.InvariantCulture)} (0x{BitConverter.SingleToUInt32Bits(single):X8})",
            double precise => $"{precise.ToString(CultureInfo.InvariantCulture)} (0x{BitConverter.DoubleToUInt64Bits(precise):X16})",
            Vector3 vector => $"({Describe(vector.X)}, {Describe(vector.Y)}, {Describe(vector.Z)})",
            Vector2 plane => $"({Describe(plane.X)}, {Describe(plane.Y)})",
            Quaternion rotation => $"({Describe(rotation.X)}, {Describe(rotation.Y)}, {Describe(rotation.Z)}, {Describe(rotation.W)})",
            byte[] bytes => $"[{Convert.ToHexString(bytes)}]",
            Enum named when ScalarValues.TryGetInteger(named, out Int128 number) => $"{named} ({number})",
            IFormattable formattable => formattable.ToString(null, CultureInfo.InvariantCulture),
            _ => value.ToString() ?? "",
        };
    }
}
