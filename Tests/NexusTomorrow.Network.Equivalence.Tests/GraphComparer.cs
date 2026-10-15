using System.Collections;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Numerics;
using System.Reflection;

namespace NexusTomorrow.Network.Equivalence.Tests;

/// <summary>
/// Compares a declared instance with a legacy instance member by member, with the twin mapper's name mapping and skips.
/// Numbers compare by value, floats and doubles by bit pattern, strings by ordinal, collections by count then element, and union arms must be twins.
/// </summary>
internal sealed class GraphComparer
{
    private const int MaxTextLength = 40;

    private readonly Func<Type, Type?> _twinOf;

    public GraphComparer(Func<Type, Type?> twinOf)
    {
        _twinOf = twinOf;
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

            CompareMember(member, MemberAccess.GetValue(member.Member, declared), MemberAccess.GetValue(twin, legacy), $"{path}.{member.Name}", differences);
        }
    }

    private void CompareValues(WireEncoding encoding, object? declared, object? legacy, string path, List<string> differences)
    {
        if (!BothPresent(declared, legacy, path, differences))
            return;

        if (encoding == WireEncoding.Struct)
        {
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

        Type? twin = _twinOf(declared.GetType());
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
            byte[] bytes => $"[{Convert.ToHexString(bytes)}]",
            Enum named when ScalarValues.TryGetInteger(named, out Int128 number) => $"{named} ({number})",
            IFormattable formattable => formattable.ToString(null, CultureInfo.InvariantCulture),
            _ => value.ToString() ?? "",
        };
    }
}
