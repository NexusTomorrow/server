using System.Collections.Immutable;

namespace NexusTomorrow.Network.Codec;

/// <summary>
/// A message's layout: its full tree (<see cref="Tree"/>), and the field widths, in order, that the writer emits on the empty path, when every string is empty, every counted array is empty and every union takes its arm for selector 0.
/// The client's writers give the same widths; its readers read an empty terminated string as the count alone, which the facts check accounts for.
/// </summary>
public sealed class WireLayout
{
    private ImmutableArray<WireNode> _tree;

    /// <summary>
    /// Create a layout; generated code calls this once per message.
    /// </summary>
    public WireLayout(ushort opcode, Direction direction, bool isFixed, bool isComplete, ImmutableArray<byte> emptyPathWidths, ImmutableArray<WirePart> parts, string tree)
    {
        Opcode = opcode;
        Direction = direction;
        IsFixed = isFixed;
        IsComplete = isComplete;
        EmptyPathWidths = emptyPathWidths;
        Parts = parts;
        TreeText = tree;

        int bits = 0;
        foreach (byte width in emptyPathWidths)
            bits += width;
        EmptyPathBits = bits;
    }

    /// <summary>
    /// The message's opcode.
    /// </summary>
    public ushort Opcode { get; }

    /// <summary>
    /// The direction or directions the message travels.
    /// </summary>
    public Direction Direction { get; }

    /// <summary>
    /// True when the layout holds no string, counted array, union or blob, so every message has <see cref="EmptyPathBits"/> bits.
    /// </summary>
    public bool IsFixed { get; }

    /// <summary>
    /// False when a union has no arm for selector 0, so the empty path stops at that union.
    /// </summary>
    public bool IsComplete { get; }

    /// <summary>
    /// The field widths on the empty path, in wire order; a string header counts as two widths (flag, length), a vector as three, and an aligned blob's alignment as one width of the bits that reach the next byte.
    /// </summary>
    public ImmutableArray<byte> EmptyPathWidths { get; }

    /// <summary>
    /// The sum of <see cref="EmptyPathWidths"/>.
    /// </summary>
    public int EmptyPathBits { get; }

    /// <summary>
    /// The computed parts in wire order: each string, counted array, union and blob, with where its header sits in <see cref="EmptyPathWidths"/>.
    /// </summary>
    public ImmutableArray<WirePart> Parts { get; }

    /// <summary>
    /// The full layout in the text form <see cref="WireTree"/> reads: every field, string, array element and union arm, in wire order.
    /// </summary>
    public string TreeText { get; }

    /// <summary>
    /// The full layout, read from <see cref="TreeText"/> on first use. Two threads reading it first both parse and store equal values, and the
    /// store is one reference, so neither sees a torn value.
    /// </summary>
    public ImmutableArray<WireNode> Tree
    {
        get
        {
            if (_tree.IsDefault)
                _tree = WireTree.Parse(TreeText);
            return _tree;
        }
    }
}
