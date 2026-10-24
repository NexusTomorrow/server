using System.Collections.Generic;
using System.Linq;

namespace NexusTomorrow.Network.Codec.Generator;

/// <summary>
/// Computes each type's empty-path layout and full tree from the models, resolving nested types, bases and union arms across the compilation.
/// </summary>
internal sealed class LayoutCalculator
{
    private static readonly LayoutModel s_unknown = new(false, false, EquatableArray<byte>.Empty, EquatableArray<PartModel>.Empty);

    private readonly Dictionary<string, TypeModel> _types;
    private readonly Dictionary<string, LayoutModel> _layouts = [];
    private readonly HashSet<string> _inProgress = [];
    private readonly Dictionary<string, List<string>> _trees = [];
    private readonly HashSet<string> _treesInProgress = [];

    public LayoutCalculator(Dictionary<string, TypeModel> types)
    {
        _types = types;
    }

    /// <summary>
    /// The arms of a union type, most derived first so a type pattern never matches a base before its derived arm.
    /// </summary>
    public List<TypeModel> ArmsOf(string? unionType)
    {
        return [.. _types.Values
            .Where(type => type.HasUnionCase && unionType is not null && type.Supertypes.Contains(unionType))
            .OrderByDescending(type => type.Supertypes.Count)
            .ThenBy(type => type.FullName, System.StringComparer.Ordinal)];
    }

    public LayoutModel LayoutOf(TypeModel type)
    {
        if (_layouts.TryGetValue(type.FullName, out LayoutModel? done))
            return done;

        // A type that contains itself has no finite empty path.
        if (!_inProgress.Add(type.FullName))
            return s_unknown;

        var builder = new Builder();
        if (type.WireBase is not null)
            builder.Append(StructLayout(type.WireBase, type.WireBaseMetadataLayout));

        var widthIndex = new Dictionary<string, int>();
        foreach (MemberModel member in type.Members)
        {
            if (!builder.IsComplete)
                break;

            widthIndex[member.Name] = builder.Widths.Count;
            switch (member.Shape)
            {
                case MemberShape.Value when member.Value is not null:
                    builder.Append(ValueLayout(member.Value));
                    break;
                case MemberShape.Array when member.Value is not null:
                    {
                        int at = member.CountFrom is not null && widthIndex.TryGetValue(member.CountFrom, out int countAt) ? countAt : builder.Widths.Count;
                        builder.AddPart(WirePartShapes.Array, at, EmptyElementBits(member.Value));
                        if (member.CountFrom is null)
                            builder.Widths.Add((byte)member.CountBits);
                        break;
                    }

                case MemberShape.FixedArray when member.Value is not null:
                    {
                        LayoutModel element = ValueLayout(member.Value);
                        for (int i = 0; i < member.FixedLength; i++)
                            builder.Append(element);
                        break;
                    }

                case MemberShape.Union:
                    {
                        int at = member.UnionSelector is not null && widthIndex.TryGetValue(member.UnionSelector, out int selectorAt) ? selectorAt : builder.Widths.Count;
                        builder.AddPart(WirePartShapes.Union, at, -1);
                        TypeModel? zero = ArmsOf(member.UnionType).FirstOrDefault(arm => arm.UnionCaseValue == 0);
                        if (zero is null)
                            builder.IsComplete = false;
                        else
                            builder.Append(LayoutOf(zero));
                        break;
                    }

                case MemberShape.Blob:
                    {
                        // Only a message holds an aligned blob (NTC011), and it is never nested (NTC009) or a union arm (NTC012), so it starts on a byte boundary and the empty path's bits so far fix the alignment.
                        int misalignment = builder.Bits % 8;
                        if (member.BlobAlign && misalignment != 0)
                            builder.Widths.Add((byte)(8 - misalignment));
                        builder.AddPart(WirePartShapes.Blob, builder.Widths.Count, -1);
                        builder.Widths.Add((byte)member.BlobLengthBits);
                        break;
                    }
                case MemberShape.Pad:
                    for (int remaining = member.PadBits; remaining > 0; remaining -= 64)
                        builder.Widths.Add((byte)System.Math.Min(64, remaining));
                    break;
            }
        }

        _inProgress.Remove(type.FullName);
        LayoutModel layout = builder.ToLayout() with { Tree = TreeOf(type) };
        _layouts[type.FullName] = layout;
        return layout;
    }

    /// <summary>
    /// A type's layout tree in the codec's text form (<c>WireTree</c>): its base's nodes, then each member's, with every struct inline.
    /// </summary>
    public string TreeOf(TypeModel type)
    {
        return string.Join(" ", NodesOf(type));
    }

    private List<string> NodesOf(TypeModel type)
    {
        if (_trees.TryGetValue(type.FullName, out List<string>? done))
            return done;

        // A type that contains itself has no finite tree.
        if (!_treesInProgress.Add(type.FullName))
            return ["?"];

        var nodes = new List<string>();
        if (type.WireBase is not null)
            AppendInline(nodes, StructNodes(type.WireBase, type.WireBaseMetadataLayout));

        var index = new Dictionary<string, int>(System.StringComparer.Ordinal);
        foreach (MemberModel member in type.Members)
        {
            index[member.Name] = nodes.Count;
            switch (member.Shape)
            {
                case MemberShape.Value when member.Value is not null:
                    AppendInline(nodes, ValueNodes(member.Value));
                    break;
                case MemberShape.Array when member.Value is not null:
                    {
                        int count;
                        if (member.CountFrom is not null && index.TryGetValue(member.CountFrom, out int countAt))
                        {
                            count = countAt;
                        }
                        else
                        {
                            count = nodes.Count;
                            nodes.Add(Number(member.CountBits));
                        }

                        nodes.Add("[" + Number(count) + ":" + string.Join(" ", ValueNodes(member.Value)) + "]");
                        break;
                    }

                case MemberShape.FixedArray when member.Value is not null:
                    {
                        List<string> element = ValueNodes(member.Value);
                        for (int i = 0; i < member.FixedLength; i++)
                            AppendInline(nodes, element);
                        break;
                    }

                case MemberShape.Union:
                    {
                        int selector = member.UnionSelector is not null && index.TryGetValue(member.UnionSelector, out int selectorAt) ? selectorAt : -1;
                        ScalarModel? scalar = type.Members.FirstOrDefault(candidate => candidate.Name == member.UnionSelector)?.Value?.Scalar;
                        IEnumerable<string> arms = ArmsOf(member.UnionType)
                            .Select(arm => (Label: WireBits(arm.UnionCaseValue, scalar), Arm: arm))
                            .OrderBy(arm => arm.Label)
                            .Select(arm => Number(arm.Label) + "=(" + string.Join(" ", NodesOf(arm.Arm)) + ")");
                        nodes.Add("{" + Number(selector) + ":" + string.Join(",", arms) + "}");
                        break;
                    }

                case MemberShape.Blob:
                    nodes.Add("b" + Number(member.BlobLengthBits) + (member.BlobAlign ? "a" : ""));
                    break;
                case MemberShape.Pad:
                    for (int remaining = member.PadBits; remaining > 0; remaining -= 64)
                        nodes.Add(Number(System.Math.Min(64, remaining)));
                    break;
            }
        }

        _treesInProgress.Remove(type.FullName);
        _trees[type.FullName] = nodes;
        return nodes;
    }

    private List<string> ValueNodes(ValueModel value)
    {
        switch (value.Kind)
        {
            case ValueKind.Scalar when value.Scalar is not null:
                return [.. Enumerable.Repeat(Number(value.Scalar.Bits) + MarkOf(value.Scalar.Encoding), value.Scalar.Components)];
            case ValueKind.String:
                return [value.String switch { StringEncoding.Wide => "w", StringEncoding.Narrow => "n", _ => "t" }];
            case ValueKind.Struct when value.StructType is not null:
                return StructNodes(value.StructType, value.StructMetadataLayout);
            default:
                return ["?"];
        }
    }

    private List<string> StructNodes(string fullName, LayoutModel? metadata)
    {
        if (metadata is not null)
            return SplitTop(metadata.Tree);
        return _types.TryGetValue(fullName, out TypeModel? type) ? NodesOf(type) : ["?"];
    }

    /// <summary>
    /// Append a struct's nodes inline: each of its top-level arrays and unions names its count or selector by index, so shift that index by
    /// where the struct starts.
    /// </summary>
    private static void AppendInline(List<string> nodes, List<string> inner)
    {
        int offset = nodes.Count;
        foreach (string node in inner)
            nodes.Add(offset == 0 ? node : Shift(node, offset));
    }

    private static string Shift(string node, int offset)
    {
        if (node.Length == 0 || (node[0] != '[' && node[0] != '{'))
            return node;
        int colon = node.IndexOf(':');
        int index = int.Parse(node.Substring(1, colon - 1), System.Globalization.CultureInfo.InvariantCulture);
        return node[0] + Number(index + offset) + node.Substring(colon);
    }

    /// <summary>
    /// The top-level nodes of a tree's text: split at the spaces outside brackets.
    /// </summary>
    private static List<string> SplitTop(string text)
    {
        var nodes = new List<string>();
        int depth = 0;
        int start = 0;
        for (int i = 0; i < text.Length; i++)
        {
            char c = text[i];
            if (c is '[' or '{' or '(')
            {
                depth++;
            }
            else if (c is ']' or '}' or ')')
            {
                depth--;
            }
            else if (c == ' ' && depth == 0)
            {
                nodes.Add(text.Substring(start, i - start));
                start = i + 1;
            }
        }

        if (text.Length > 0)
            nodes.Add(text.Substring(start));
        return nodes;
    }

    /// <summary>
    /// A case value as the selector's bits read unsigned, which is how the client's tree keys an arm: a negative [SInt] case is its two's
    /// complement in the selector's width. <see cref="UnionCases.Label"/> is not this: it masks to the selector's C# type.
    /// </summary>
    private static ulong WireBits(ulong value, ScalarModel? selector)
    {
        return selector is null || selector.Bits >= 64 ? value : value & ((1UL << selector.Bits) - 1);
    }

    // The field's encoding in the tree's text form (WireTree): nothing for an unsigned integer.
    private static string MarkOf(ScalarEncoding encoding) => encoding switch
    {
        ScalarEncoding.SInt => "s",
        ScalarEncoding.Float32 or ScalarEncoding.Float64 => "f",
        ScalarEncoding.PackedFloat => "h",
        _ => "",
    };

    private static string Number(int value)
    {
        return value.ToString(System.Globalization.CultureInfo.InvariantCulture);
    }

    private static string Number(ulong value)
    {
        return value.ToString(System.Globalization.CultureInfo.InvariantCulture);
    }

    public LayoutModel ValueLayout(ValueModel value)
    {
        var builder = new Builder();
        switch (value.Kind)
        {
            case ValueKind.Scalar when value.Scalar is not null:
                for (int i = 0; i < value.Scalar.Components; i++)
                    builder.Widths.Add((byte)value.Scalar.Bits);
                break;
            case ValueKind.String:
                builder.AddPart(value.String switch
                {
                    StringEncoding.Wide => WirePartShapes.WideString,
                    StringEncoding.Narrow => WirePartShapes.NarrowString,
                    _ => WirePartShapes.TerminatedWideString,
                }, 0, -1);
                // An empty terminated string is its count (1) and the terminator: the client's writer emits 16, 16 for it (0x0592).
                if (value.String == StringEncoding.Terminated)
                {
                    builder.Widths.Add(16);
                    builder.Widths.Add(16);
                }
                else
                {
                    builder.Widths.Add(1);
                    builder.Widths.Add(7);
                }

                break;
            case ValueKind.Struct when value.StructType is not null:
                builder.Append(StructLayout(value.StructType, value.StructMetadataLayout));
                break;
        }

        return builder.ToLayout();
    }

    /// <summary>
    /// The bits one element takes on the empty path, or -1 when that path is incomplete.
    /// </summary>
    public int EmptyElementBits(ValueModel value)
    {
        LayoutModel layout = ValueLayout(value);
        return layout.IsComplete ? layout.Bits : -1;
    }

    /// <summary>
    /// The bits each element adds when its layout is fixed, else -1.
    /// </summary>
    public int ElementBits(ValueModel value)
    {
        LayoutModel layout = ValueLayout(value);
        return layout.IsFixed && layout.IsComplete ? layout.Bits : -1;
    }

    /// <summary>
    /// A lower bound on the bits any one element takes: its empty path up to and including the first union selector, which every arm follows,
    /// less each terminated string's terminator, since a reader takes a count of 0 as the empty string (BitReader.ReadTerminatedWideString).
    /// </summary>
    public int GuaranteedBits(ValueModel value)
    {
        LayoutModel layout = ValueLayout(value);
        PartModel? union = layout.Parts.FirstOrDefault(part => part.Shape == WirePartShapes.Union);
        int end = union is null ? (layout.IsComplete ? layout.Widths.Count : 0) : union.At + 1;
        int bits = 0;
        for (int i = 0; i < end && i < layout.Widths.Count; i++)
            bits += layout.Widths[i];
        foreach (PartModel terminated in layout.Parts.Where(part => part.Shape == WirePartShapes.TerminatedWideString && part.At + 1 < end))
            bits -= layout.Widths[terminated.At + 1];
        return bits;
    }

    private LayoutModel StructLayout(string fullName, LayoutModel? metadata)
    {
        if (metadata is not null)
            return metadata;
        return _types.TryGetValue(fullName, out TypeModel? type) ? LayoutOf(type) : s_unknown;
    }

    private sealed class Builder
    {
        public List<byte> Widths { get; } = [];

        public List<PartModel> Parts { get; } = [];

        public bool IsFixed { get; set; } = true;

        public bool IsComplete { get; set; } = true;

        public int Bits => Widths.Sum(width => (int)width);

        public void AddPart(int shape, int at, int elementBits)
        {
            Parts.Add(new PartModel(shape, at, elementBits));
            IsFixed = false;
        }

        public void Append(LayoutModel inner)
        {
            int offset = Widths.Count;
            Widths.AddRange(inner.Widths);
            foreach (PartModel part in inner.Parts)
                Parts.Add(part with { At = part.At + offset });
            IsFixed &= inner.IsFixed;
            IsComplete &= inner.IsComplete;
        }

        public LayoutModel ToLayout()
        {
            return new LayoutModel(IsFixed, IsComplete, new EquatableArray<byte>([.. Widths]), new EquatableArray<PartModel>([.. Parts]));
        }
    }
}

/// <summary>
/// The numeric values of <c>NexusTomorrow.Network.Codec.WirePartShape</c>, which the generator cannot reference.
/// </summary>
internal static class WirePartShapes
{
    public const int WideString = 1;
    public const int NarrowString = 2;
    public const int TerminatedWideString = 3;
    public const int Array = 4;
    public const int Union = 5;
    public const int Blob = 6;
}
