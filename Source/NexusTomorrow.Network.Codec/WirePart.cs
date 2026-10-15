namespace NexusTomorrow.Network.Codec;

/// <summary>
/// A computed part of a layout: a member whose size depends on the message.
/// </summary>
public readonly record struct WirePart
{
    /// <summary>
    /// What kind of computed member this is.
    /// </summary>
    public required WirePartShape Shape { get; init; }

    /// <summary>
    /// The index in <see cref="WireLayout.EmptyPathWidths"/> of the part's first width: a string's flag, an array's count, a union's selector, a blob's length.
    /// </summary>
    public required int At { get; init; }

    /// <summary>
    /// For an array, the bits one element takes on the empty path, as the client's size function counts it; -1 for other parts or when that path is incomplete.
    /// </summary>
    public required int ElementBits { get; init; }
}
