using System.Collections.Immutable;

namespace NexusTomorrow.Network.Codec;

/// <summary>
/// The kinds of node in a layout tree (<see cref="WireTree"/>).
/// </summary>
public enum WireNodeKind
{
    /// <summary>
    /// A scalar of <see cref="WireNode.Bits"/> bits; selectors and counts are fields too.
    /// </summary>
    Field = 1,

    /// <summary>
    /// A string with a 1-bit form flag, a 7 or 15-bit length and that many UTF-16 code units.
    /// </summary>
    WideString = 2,

    /// <summary>
    /// A string with a 1-bit form flag, a 7 or 15-bit length and that many bytes.
    /// </summary>
    NarrowString = 3,

    /// <summary>
    /// A string with a 16-bit count that includes its terminator, then that many UTF-16 code units.
    /// </summary>
    TerminatedWideString = 4,

    /// <summary>
    /// A counted array: the field at <see cref="WireNode.Reference"/> holds the count, and each element has the layout <see cref="WireNode.Element"/>.
    /// </summary>
    Array = 5,

    /// <summary>
    /// A union: the field at <see cref="WireNode.Reference"/> selects one of <see cref="WireNode.Arms"/>.
    /// </summary>
    Union = 6,

    /// <summary>
    /// A run of bytes after a <see cref="WireNode.Bits"/>-bit length, starting on a byte when <see cref="WireNode.Aligned"/>.
    /// </summary>
    Blob = 7,

    /// <summary>
    /// A layout that contains itself, which the tree does not expand.
    /// </summary>
    Recursive = 8,
}

/// <summary>
/// One arm of a union: the selector value that chooses it and the layout that follows.
/// </summary>
public sealed class WireArm
{
    /// <summary>
    /// Create an arm.
    /// </summary>
    public WireArm(ulong selector, ImmutableArray<WireNode> nodes)
    {
        Selector = selector;
        Nodes = nodes;
    }

    /// <summary>
    /// The selector value, as its bits read unsigned.
    /// </summary>
    public ulong Selector { get; }

    /// <summary>
    /// The arm's layout, in wire order.
    /// </summary>
    public ImmutableArray<WireNode> Nodes { get; }
}

/// <summary>
/// One node of a layout tree. A struct's members sit inline in the sequence that holds the struct, so only arrays and unions nest.
/// </summary>
public sealed class WireNode
{
    private WireNode(WireNodeKind kind, int bits, bool aligned, int reference, ImmutableArray<WireNode> element, ImmutableArray<WireArm> arms)
    {
        Kind = kind;
        Bits = bits;
        Aligned = aligned;
        Reference = reference;
        Element = element;
        Arms = arms;
    }

    /// <summary>
    /// The node that stands for a layout containing itself.
    /// </summary>
    public static WireNode Recursive { get; } = new(WireNodeKind.Recursive, 0, false, -1, [], []);

    /// <summary>
    /// What the node is.
    /// </summary>
    public WireNodeKind Kind { get; }

    /// <summary>
    /// A field's width, or a blob's length width, in bits; 0 for every other kind.
    /// </summary>
    public int Bits { get; }

    /// <summary>
    /// True for a blob that starts on a byte.
    /// </summary>
    public bool Aligned { get; }

    /// <summary>
    /// For an array, the index of its count field in the same sequence; for a union, the index of its selector field; -1 otherwise.
    /// </summary>
    public int Reference { get; }

    /// <summary>
    /// An array's element layout; empty for every other kind.
    /// </summary>
    public ImmutableArray<WireNode> Element { get; }

    /// <summary>
    /// A union's arms in ascending selector order; empty for every other kind.
    /// </summary>
    public ImmutableArray<WireArm> Arms { get; }

    /// <summary>
    /// A field of <paramref name="bits"/> bits, 1 to 64.
    /// </summary>
    public static WireNode Field(int bits)
    {
        ArgumentOutOfRangeException.ThrowIfLessThan(bits, 1);
        ArgumentOutOfRangeException.ThrowIfGreaterThan(bits, 64);
        return new WireNode(WireNodeKind.Field, bits, false, -1, [], []);
    }

    /// <summary>
    /// A string of one of the three string kinds.
    /// </summary>
    public static WireNode Text(WireNodeKind kind)
    {
        if (kind is not (WireNodeKind.WideString or WireNodeKind.NarrowString or WireNodeKind.TerminatedWideString))
            throw new ArgumentOutOfRangeException(nameof(kind), kind, "Not a string kind.");
        return new WireNode(kind, 0, false, -1, [], []);
    }

    /// <summary>
    /// An array counted by the field at <paramref name="count"/>.
    /// </summary>
    public static WireNode Array(int count, ImmutableArray<WireNode> element)
    {
        return new WireNode(WireNodeKind.Array, 0, false, count, element, []);
    }

    /// <summary>
    /// A union selected by the field at <paramref name="selector"/>.
    /// </summary>
    public static WireNode Union(int selector, ImmutableArray<WireArm> arms)
    {
        return new WireNode(WireNodeKind.Union, 0, false, selector, [], arms);
    }

    /// <summary>
    /// A blob with a <paramref name="lengthBits"/>-bit length.
    /// </summary>
    public static WireNode Blob(int lengthBits, bool aligned)
    {
        ArgumentOutOfRangeException.ThrowIfLessThan(lengthBits, 1);
        ArgumentOutOfRangeException.ThrowIfGreaterThan(lengthBits, 64);
        return new WireNode(WireNodeKind.Blob, lengthBits, aligned, -1, [], []);
    }
}
