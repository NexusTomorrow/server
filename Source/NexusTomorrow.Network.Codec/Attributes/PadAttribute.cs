namespace NexusTomorrow.Network.Codec;

/// <summary>
/// A <see cref="Pad"/> member of <see cref="Bits"/> reserved bits: zeros on write, skipped on read.
/// </summary>
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
public sealed class PadAttribute : Attribute
{
    /// <summary>
    /// Declare the number of reserved bits.
    /// </summary>
    public PadAttribute(int bits)
    {
        Bits = bits;
    }

    /// <summary>
    /// The number of reserved bits.
    /// </summary>
    public int Bits { get; }
}
