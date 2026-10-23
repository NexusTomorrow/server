using System.Globalization;
using System.Numerics;
using System.Reflection;
using NexusTomorrow.Network.Codec;

namespace NexusTomorrow.Tools;

/// <summary>
/// A message's members as rows in read order, for the <c>layout</c> verb: each row's path, width in bits (empty for a header,
/// <c>varies</c> for a string or a byte block) and encoding. The members are <see cref="MessageBinder"/>'s, so a row path is the
/// key the binder takes; a struct opens with a <c>the fields below</c> row, a fixed array has a row per element, an array counted
/// on the wire is its own row followed by one element's rows under <c>path[]</c>, and a union is one row that says so, since its
/// rows depend on the values sent.
/// </summary>
internal static class LayoutRows
{
    private static readonly string[] s_components = ["X", "Y", "Z", "W"];

    public static List<(string Path, string Bits, string Encoding)> Of(Type type)
    {
        var rows = new List<(string Path, string Bits, string Encoding)>();
        Append(type, "", rows, []);
        return rows;
    }

    private static void Append(Type type, string prefix, List<(string, string, string)> rows, HashSet<Type> open)
    {
        if (!open.Add(type))
        {
            rows.Add((prefix.TrimEnd('.'), "", "this layout again"));
            return;
        }

        foreach (MemberInfo member in MessageBinder.WireMembers(type))
        {
            string path = prefix + member.Name;
            Type memberType = MessageBinder.MemberType(member);
            (string Bits, string Encoding)? single = member switch
            {
                _ when member.GetCustomAttribute<PadAttribute>() is { } pad => (Number(pad.Bits), "padding"),
                _ when member.GetCustomAttribute<BlobAttribute>() is not null => ("varies", "byte block"),
                _ when member.GetCustomAttribute<UnionAttribute>() is { } union => ("", $"union, selected by `{prefix}{union.Selector}`"),
                _ => null,
            };
            if (single is { } row)
            {
                rows.Add((path, row.Bits, row.Encoding));
                continue;
            }

            if (member.GetCustomAttribute<ArrayAttribute>() is not null)
            {
                rows.Add((path, "", "array, counted on the wire"));
                Type element = memberType.IsArray ? memberType.GetElementType()! : memberType.GetGenericArguments().Single();
                AppendValue(member, element, path + "[]", rows, open);
                continue;
            }

            if (member.GetCustomAttribute<FixedArrayAttribute>() is { } fixedArray)
            {
                Type element = memberType.GetElementType()!;
                for (int i = 0; i < fixedArray.Length; i++)
                    AppendValue(member, element, $"{path}[{Number(i)}]", rows, open);
                continue;
            }

            AppendValue(member, memberType, path, rows, open);
        }

        open.Remove(type);
    }

    private static void AppendValue(MemberInfo member, Type valueType, string path, List<(string, string, string)> rows, HashSet<Type> open)
    {
        int components = valueType == typeof(Quaternion) ? 4 : valueType == typeof(Vector3) ? 3 : valueType == typeof(Vector2) ? 2 : 1;
        (int Bits, string Encoding)? scalar = member switch
        {
            _ when member.GetCustomAttribute<UIntAttribute>() is { } unsigned => (unsigned.Bits, "unsigned"),
            _ when member.GetCustomAttribute<SIntAttribute>() is { } signed => (signed.Bits, "signed"),
            _ when member.GetCustomAttribute<Float32Attribute>() is not null => (32, "float"),
            _ when member.GetCustomAttribute<Float64Attribute>() is not null => (64, "double"),
            _ when member.GetCustomAttribute<PackedFloatAttribute>() is not null => (16, "packed float"),
            _ => null,
        };
        if (scalar is { } field)
        {
            for (int i = 0; i < components; i++)
                rows.Add((components == 1 ? path : $"{path}.{s_components[i]}", Number(field.Bits), field.Encoding));
            return;
        }

        string? text = member switch
        {
            _ when member.GetCustomAttribute<WideStringAttribute>() is not null => "wide string",
            _ when member.GetCustomAttribute<NarrowStringAttribute>() is not null => "narrow string",
            _ when member.GetCustomAttribute<TerminatedWideStringAttribute>() is not null => "terminated wide string",
            _ => null,
        };
        if (text is not null)
        {
            rows.Add((path, "varies", text));
            return;
        }

        if (MessageBinder.IsWireType(valueType) && !valueType.IsAbstract)
        {
            rows.Add((path, "", "the fields below"));
            Append(valueType, path + ".", rows, open);
            return;
        }

        throw new InvalidOperationException($"{path}: no wire shape for {valueType.Name}");
    }

    private static string Number(int value) => value.ToString(CultureInfo.InvariantCulture);
}
