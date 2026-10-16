namespace NexusTomorrow.Network.Codec;

/// <summary>
/// The generator puts this on every type it implements, so a layout in another assembly can nest the type without its source. Not for hand use.
/// </summary>
[AttributeUsage(AttributeTargets.Class, Inherited = false)]
public sealed class WireLayoutDataAttribute : Attribute
{
    /// <summary>
    /// Record a type's empty-path layout and its full tree.
    /// </summary>
    /// <param name="isFixed">True when the layout holds no computed part.</param>
    /// <param name="isComplete">False when a union has no arm for selector 0.</param>
    /// <param name="widths">The empty-path field widths in order.</param>
    /// <param name="parts">Each computed part as three numbers: shape, index of its first width, element bits or -1.</param>
    /// <param name="tree">The layout tree in <see cref="WireTree"/>'s text form.</param>
    public WireLayoutDataAttribute(bool isFixed, bool isComplete, byte[] widths, int[] parts, string tree)
    {
        IsFixed = isFixed;
        IsComplete = isComplete;
        Widths = widths;
        Parts = parts;
        Tree = tree;
    }

    /// <summary>
    /// True when the layout holds no computed part.
    /// </summary>
    public bool IsFixed { get; }

    /// <summary>
    /// False when a union has no arm for selector 0.
    /// </summary>
    public bool IsComplete { get; }

    /// <summary>
    /// The empty-path field widths in order.
    /// </summary>
    public IReadOnlyList<byte> Widths { get; }

    /// <summary>
    /// Each computed part as three numbers: shape, index of its first width, element bits or -1.
    /// </summary>
    public IReadOnlyList<int> Parts { get; }

    /// <summary>
    /// The layout tree in <see cref="WireTree"/>'s text form.
    /// </summary>
    public string Tree { get; }
}
