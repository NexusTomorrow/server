using System.Collections.Immutable;

namespace NexusTomorrow.Network.Codec;

/// <summary>
/// A message's layout on the empty path: the field widths, in order, that the writer emits when every string is empty, every counted array is empty and every union takes its arm for selector 0.
/// The client's writers give the same widths; its readers read an empty terminated string as the count alone, which the facts check accounts for.
/// </summary>
public sealed class WireLayout
{
    /// <summary>
    /// Create a layout; generated code calls this once per message.
    /// </summary>
    public WireLayout(ushort opcode, Direction direction, bool isFixed, bool isComplete, ImmutableArray<byte> emptyPathWidths, ImmutableArray<WirePart> parts)
    {
        Opcode = opcode;
        Direction = direction;
        IsFixed = isFixed;
        IsComplete = isComplete;
        EmptyPathWidths = emptyPathWidths;
        Parts = parts;

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
}
