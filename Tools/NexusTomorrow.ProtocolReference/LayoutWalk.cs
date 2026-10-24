using System.Collections.Immutable;
using System.Globalization;
using System.Numerics;
using System.Reflection;
using System.Runtime.CompilerServices;
using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.ProtocolReference;

/// <summary>
/// One row of a message's field table: where the field sits, its width, how it is encoded, and the documentation id its meaning comes from.
/// </summary>
public sealed record LayoutRow(string Path, string Bits, string Encoding, string? DocumentationId);

/// <summary>
/// A layout walked: its tree, as the generator writes it, and a row for every node.
/// </summary>
public sealed record LayoutWalkResult(ImmutableArray<WireNode> Nodes, ImmutableArray<LayoutRow> Rows);

/// <summary>
/// Walks a declared layout by reflection into the generator's tree and a named row per node. The order is the generator's: a base layout's
/// nodes first, then each wire member as declared, every struct inline. Nodes go into the sequence that holds them, so a count or selector
/// reference is its absolute index there; an array's element and each union arm are sequences of their own.
/// </summary>
public sealed class LayoutWalk
{
    private static readonly string[] s_components = ["X", "Y", "Z", "W"];

    private readonly Dictionary<Assembly, Type[]> _armsByAssembly = [];

    /// <summary>
    /// Create a walk over the declarations in <paramref name="models"/>. A union's arms are the <see cref="UnionCaseAttribute"/> types in the
    /// assembly that declares the union member, as the generator, which sees one compilation, finds them.
    /// </summary>
    public LayoutWalk(IReadOnlyCollection<Assembly> models)
    {
        foreach (Assembly model in models)
            _armsByAssembly[model] = [.. model.GetTypes().Where(type => type.GetCustomAttribute<UnionCaseAttribute>() is not null)];
    }

    /// <summary>
    /// Walk <paramref name="layout"/>.
    /// </summary>
    /// <exception cref="InvalidOperationException">A member has no wire shape.</exception>
    public LayoutWalkResult Walk(Type layout)
    {
        var nodes = new List<WireNode>();
        var rows = new List<LayoutRow>();
        AppendType(layout, "", nodes, rows, []);
        return new LayoutWalkResult([.. nodes], [.. rows]);
    }

    private void AppendType(Type type, string prefix, List<WireNode> nodes, List<LayoutRow> rows, HashSet<Type> open)
    {
        open.Add(type);
        if (type.BaseType is { } baseType && IsWireType(baseType))
            AppendStructInline(baseType, prefix, nodes, rows, open);

        var nodeOf = new Dictionary<string, int>(StringComparer.Ordinal);
        var rowOf = new Dictionary<string, int>(StringComparer.Ordinal);
        var countedBy = new Dictionary<string, List<string>>(StringComparer.Ordinal);
        var selects = new Dictionary<string, List<string>>(StringComparer.Ordinal);
        foreach (MemberInfo member in WireMembersOf(type))
        {
            string path = prefix + member.Name;
            nodeOf[member.Name] = nodes.Count;
            rowOf[member.Name] = rows.Count;
            AppendMember(type, member, path, prefix, nodes, rows, open, nodeOf, countedBy, selects);
        }

        // A count or selector row names what it counts or selects.
        foreach ((string name, List<string> arrays) in countedBy)
        {
            if (rowOf.TryGetValue(name, out int at))
                rows[at] = rows[at] with { Encoding = $"{rows[at].Encoding}; counts {string.Join(", ", arrays)}" };
        }

        foreach ((string name, List<string> unions) in selects)
        {
            if (rowOf.TryGetValue(name, out int at))
                rows[at] = rows[at] with { Encoding = $"{rows[at].Encoding}; selects {string.Join(", ", unions)}" };
        }

        open.Remove(type);
    }

    private void AppendMember(
        Type type,
        MemberInfo member,
        string path,
        string prefix,
        List<WireNode> nodes,
        List<LayoutRow> rows,
        HashSet<Type> open,
        Dictionary<string, int> nodeOf,
        Dictionary<string, List<string>> countedBy,
        Dictionary<string, List<string>> selects)
    {
        Type memberType = MemberType(member);
        string id = DocumentationIds.Of(member);

        if (member.GetCustomAttribute<PadAttribute>() is { } pad)
        {
            var chunks = new List<string>();
            for (int remaining = pad.Bits; remaining > 0; remaining -= 64)
            {
                int bits = Math.Min(64, remaining);
                nodes.Add(WireNode.Field(bits));
                chunks.Add(Number(bits));
            }

            rows.Add(new LayoutRow(path, string.Join(" ", chunks), "padding", id));
            return;
        }

        if (member.GetCustomAttribute<BlobAttribute>() is { } blob)
        {
            nodes.Add(WireNode.Blob(blob.LengthBits, blob.Align));
            string encoding = $"byte block, {Number(blob.LengthBits)}-bit length"
                + (blob.Align ? ", byte-aligned" : "")
                + (blob.LengthIncludesSelf ? ", the length counts itself" : "");
            rows.Add(new LayoutRow(path, "varies", encoding, id));
            return;
        }

        if (member.GetCustomAttribute<UnionAttribute>() is { } union)
        {
            AppendUnion(type, member, memberType, union.Selector, path, prefix, nodes, rows, open, nodeOf, selects, id);
            return;
        }

        if (member.GetCustomAttribute<ArrayAttribute>() is { } array)
        {
            Type element = ElementType(memberType)
                ?? throw new InvalidOperationException($"{type.Name}.{member.Name}: an [Array] member is a List<T> or a T[].");
            string countPath;
            int countAt;
            if (array.CountFrom is { } countFrom && nodeOf.TryGetValue(countFrom, out int countFromAt))
            {
                countAt = countFromAt;
                countPath = prefix + countFrom;
                if (!countedBy.TryGetValue(countFrom, out List<string>? arrays))
                    countedBy[countFrom] = arrays = [];
                arrays.Add($"`{path}`");
            }
            else
            {
                countAt = nodes.Count;
                countPath = path + " count";
                nodes.Add(WireNode.Field(array.CountBits));
                rows.Add(new LayoutRow(countPath, Number(array.CountBits), "unsigned", null));
            }

            rows.Add(new LayoutRow(path, "", $"array, counted by `{countPath}`", id));
            var elementNodes = new List<WireNode>();

            // The array's header row carries the member's meaning; its element row would only repeat it.
            AppendValue(type, member, element, path + "[]", elementNodes, rows, open, id: null, header: false);
            nodes.Add(WireNode.Array(countAt, [.. elementNodes]));
            return;
        }

        if (member.GetCustomAttribute<FixedArrayAttribute>() is { } fixedArray)
        {
            Type element = memberType.GetElementType()
                ?? throw new InvalidOperationException($"{type.Name}.{member.Name}: a [FixedArray] member is a T[].");
            // The first slot carries the member's meaning; the rest would only repeat it.
            for (int i = 0; i < fixedArray.Length; i++)
                AppendValue(type, member, element, $"{path}[{Number(i)}]", nodes, rows, open, i == 0 ? id : null, header: true);
            return;
        }

        AppendValue(type, member, memberType, path, nodes, rows, open, id, header: true);
    }

    // One value: a scalar (one node per component), a string, or a struct inline. A struct used as an array element has no header row of its
    // own; its fields carry the element's path.
    private void AppendValue(Type owner, MemberInfo member, Type valueType, string path, List<WireNode> nodes, List<LayoutRow> rows, HashSet<Type> open, string? id, bool header)
    {
        if (Scalar(member, valueType) is { } scalar)
        {
            if (scalar.Components == 1)
            {
                nodes.Add(WireNode.Field(scalar.Bits, scalar.Wire));
                rows.Add(new LayoutRow(path, Number(scalar.Bits), scalar.Encoding, id));
                return;
            }

            // The first component carries the member's meaning; the rest would only repeat it.
            for (int i = 0; i < scalar.Components; i++)
            {
                nodes.Add(WireNode.Field(scalar.Bits, scalar.Wire));
                rows.Add(new LayoutRow($"{path}.{s_components[i]}", Number(scalar.Bits), scalar.Encoding, i == 0 ? id : null));
            }

            return;
        }

        if (Text(member) is { } text)
        {
            nodes.Add(WireNode.Text(text.Kind));
            rows.Add(new LayoutRow(path, "varies", text.Encoding, id));
            return;
        }

        if (IsWireType(valueType) && !valueType.IsAbstract)
        {
            if (open.Contains(valueType))
            {
                nodes.Add(WireNode.Recursive);
                rows.Add(new LayoutRow(path, "", "this layout again", id));
                return;
            }

            if (header)
                rows.Add(new LayoutRow(path, "", "the fields below", id));
            AppendStructInline(valueType, path + ".", nodes, rows, open);
            return;
        }

        throw new InvalidOperationException($"{owner.Name}.{member.Name}: no wire shape for {valueType.Name}.");
    }

    private void AppendStructInline(Type type, string prefix, List<WireNode> nodes, List<LayoutRow> rows, HashSet<Type> open)
    {
        if (open.Contains(type))
        {
            nodes.Add(WireNode.Recursive);
            rows.Add(new LayoutRow(prefix.TrimEnd('.'), "", "this layout again", DocumentationIds.Of(type)));
            return;
        }

        AppendType(type, prefix, nodes, rows, open);
    }

    private void AppendUnion(
        Type type,
        MemberInfo member,
        Type unionType,
        string selector,
        string path,
        string prefix,
        List<WireNode> nodes,
        List<LayoutRow> rows,
        HashSet<Type> open,
        Dictionary<string, int> nodeOf,
        Dictionary<string, List<string>> selects,
        string id)
    {
        int selectorAt = nodeOf.TryGetValue(selector, out int at) ? at : -1;
        MemberInfo? selectorMember = WireMembersOf(type).FirstOrDefault(candidate => candidate.Name == selector);
        (int Bits, bool Signed)? width = selectorMember is null ? null : IntegerWidth(selectorMember);
        if (selectorAt >= 0)
        {
            if (!selects.TryGetValue(selector, out List<string>? unions))
                selects[selector] = unions = [];
            unions.Add($"`{path}`");
        }

        string selectorPath = prefix + selector;
        rows.Add(new LayoutRow(path, "", $"union, selected by `{selectorPath}`", id));

        Type declaring = member.DeclaringType ?? type;
        IEnumerable<(ulong Label, Type Arm)> arms = (_armsByAssembly.TryGetValue(declaring.Assembly, out Type[]? candidates) ? candidates : CasesIn(declaring.Assembly))
            .Where(arm => arm != unionType && unionType.IsAssignableFrom(arm))
            .Select(arm => (Label: WireBits(CaseValue(arm), width), Arm: arm))
            .OrderBy(arm => arm.Label);

        ImmutableArray<WireArm>.Builder wireArms = ImmutableArray.CreateBuilder<WireArm>();
        foreach ((ulong label, Type arm) in arms)
        {
            string value = Selected(label, width);
            var armNodes = new List<WireNode>();
            var armRows = new List<LayoutRow>();
            AppendType(arm, $"{path}({value}).", armNodes, armRows, open);
            string encoding = $"when `{selectorPath}` is {value}" + (armNodes.Count == 0 ? "; nothing more" : "");
            rows.Add(new LayoutRow($"{path}({value})", "", encoding, DocumentationIds.Of(arm)));
            rows.AddRange(armRows);
            wireArms.Add(new WireArm(label, [.. armNodes]));
        }

        nodes.Add(WireNode.Union(selectorAt, wireArms.ToImmutable()));
    }

    private Type[] CasesIn(Assembly assembly)
    {
        Type[] cases = [.. assembly.GetTypes().Where(type => type.GetCustomAttribute<UnionCaseAttribute>() is not null)];
        _armsByAssembly[assembly] = cases;
        return cases;
    }

    // The members the generator puts on the wire: instance properties (not indexers or explicit interface implementations) and fields,
    // declared in source, without [NotOnWire], in declaration order.
    private static List<MemberInfo> WireMembersOf(Type type)
    {
        const BindingFlags Declared = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.DeclaredOnly;
        List<MemberInfo> properties = [.. type.GetProperties(Declared)
            .Where(property => property.GetIndexParameters().Length == 0 && !property.Name.Contains('.', StringComparison.Ordinal))
            .Where(property => property.GetCustomAttribute<NotOnWireAttribute>() is null)
            .OrderBy(property => property.MetadataToken)];
        List<MemberInfo> fields = [.. type.GetFields(Declared)
            .Where(field => field.GetCustomAttribute<CompilerGeneratedAttribute>() is null && field.GetCustomAttribute<NotOnWireAttribute>() is null)
            .OrderBy(field => field.MetadataToken)];

        // Properties and fields sit in separate metadata tables, so their tokens cannot order one against the other.
        if (properties.Count > 0 && fields.Count > 0)
            throw new InvalidOperationException($"{type.Name} puts both fields and properties on the wire; their order cannot be read back by reflection.");
        return properties.Count > 0 ? properties : fields;
    }

    private static Type MemberType(MemberInfo member) => member switch
    {
        PropertyInfo property => property.PropertyType,
        FieldInfo field => field.FieldType,
        _ => throw new InvalidOperationException($"{member.Name} is not a property or a field."),
    };

    private static Type? ElementType(Type collection)
    {
        if (collection.IsArray)
            return collection.GetElementType();
        return collection.IsGenericType && collection.GetGenericTypeDefinition() == typeof(List<>) ? collection.GetGenericArguments()[0] : null;
    }

    private static bool IsWireType(Type type) =>
        type.GetCustomAttribute<WireStructAttribute>() is not null || type.GetCustomAttribute<WireMessageAttribute>() is not null;

    private static (int Bits, string Encoding, int Components, WireEncoding Wire)? Scalar(MemberInfo member, Type valueType)
    {
        int components = valueType == typeof(Quaternion) ? 4 : valueType == typeof(Vector3) ? 3 : valueType == typeof(Vector2) ? 2 : 1;
        if (member.GetCustomAttribute<UIntAttribute>() is { } unsigned)
            return (unsigned.Bits, "unsigned", 1, WireEncoding.Plain);
        if (member.GetCustomAttribute<SIntAttribute>() is { } signed)
            return (signed.Bits, "signed", 1, WireEncoding.TwosComplement);
        if (member.GetCustomAttribute<Float32Attribute>() is not null)
            return (32, "float", components, WireEncoding.Ieee);
        if (member.GetCustomAttribute<Float64Attribute>() is not null)
            return (64, "double", components, WireEncoding.Ieee);
        if (member.GetCustomAttribute<PackedFloatAttribute>() is not null)
            return (16, "packed float", components, WireEncoding.PackedFloat);
        return null;
    }

    private static (WireNodeKind Kind, string Encoding)? Text(MemberInfo member)
    {
        if (member.GetCustomAttribute<WideStringAttribute>() is not null)
            return (WireNodeKind.WideString, "wide string");
        if (member.GetCustomAttribute<NarrowStringAttribute>() is not null)
            return (WireNodeKind.NarrowString, "narrow string");
        if (member.GetCustomAttribute<TerminatedWideStringAttribute>() is not null)
            return (WireNodeKind.TerminatedWideString, "terminated wide string");
        return null;
    }

    private static (int Bits, bool Signed)? IntegerWidth(MemberInfo member)
    {
        if (member.GetCustomAttribute<UIntAttribute>() is { } unsigned)
            return (unsigned.Bits, false);
        if (member.GetCustomAttribute<SIntAttribute>() is { } signed)
            return (signed.Bits, true);
        return null;
    }

    // A case value as the selector's bits read unsigned, which keys the arm in the tree: a negative case is its two's complement in the
    // selector's width.
    private static ulong WireBits(ulong value, (int Bits, bool Signed)? width) =>
        width is not { Bits: < 64 } known ? value : value & ((1UL << known.Bits) - 1);

    // The selector's value as its own encoding reads it.
    private static string Selected(ulong label, (int Bits, bool Signed)? width)
    {
        if (width is { Signed: true, Bits: < 64 } signed && (label & (1UL << (signed.Bits - 1))) != 0)
            return ((long)label - (1L << signed.Bits)).ToString(CultureInfo.InvariantCulture);
        return label.ToString(CultureInfo.InvariantCulture);
    }

    private static ulong CaseValue(Type arm)
    {
        object value = arm.GetCustomAttribute<UnionCaseAttribute>()!.Value;
        object raw = value is Enum ? Convert.ChangeType(value, Enum.GetUnderlyingType(value.GetType()), CultureInfo.InvariantCulture) : value;
        return raw switch
        {
            sbyte number => unchecked((ulong)number),
            short number => unchecked((ulong)number),
            int number => unchecked((ulong)number),
            long number => unchecked((ulong)number),
            byte number => number,
            ushort number => number,
            uint number => number,
            ulong number => number,
            _ => throw new InvalidOperationException($"{arm.Name}: a [UnionCase] value is an integer or an enum."),
        };
    }

    private static string Number(int value) => value.ToString(CultureInfo.InvariantCulture);
}
