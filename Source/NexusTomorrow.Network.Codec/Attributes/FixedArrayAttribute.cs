namespace NexusTomorrow.Network.Codec;

/// <summary>
/// An array member (<c>T[]</c>) of exactly <see cref="Length"/> elements with no count on the wire.
/// </summary>
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
public sealed class FixedArrayAttribute : Attribute
{
    /// <summary>
    /// Declare the element count.
    /// </summary>
    public FixedArrayAttribute(int length)
    {
        Length = length;
    }

    /// <summary>
    /// The element count.
    /// </summary>
    public int Length { get; }
}
