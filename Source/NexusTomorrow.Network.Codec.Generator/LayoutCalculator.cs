using System.Collections.Generic;
using System.Linq;

namespace NexusTomorrow.Network.Codec.Generator;

/// <summary>
/// Computes each type's empty-path layout from the models, resolving nested types, bases and union arms across the compilation.
/// </summary>
internal sealed class LayoutCalculator
{
    private static readonly LayoutModel s_unknown = new(false, false, EquatableArray<byte>.Empty, EquatableArray<PartModel>.Empty);

    private readonly Dictionary<string, TypeModel> _types;
    private readonly Dictionary<string, LayoutModel> _layouts = [];
    private readonly HashSet<string> _inProgress = [];

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
                        // Only a message, which starts on a byte boundary, holds an aligned blob (NTC011), so the empty path's bits so far fix the alignment.
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
        LayoutModel layout = builder.ToLayout();
        _layouts[type.FullName] = layout;
        return layout;
    }

    public LayoutModel ValueLayout(ValueModel value)
    {
        var builder = new Builder();
        switch (value.Kind)
        {
            case ValueKind.Scalar when value.Scalar is not null:
                for (int i = 0; i < (value.Scalar.IsVector3 ? 3 : 1); i++)
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
