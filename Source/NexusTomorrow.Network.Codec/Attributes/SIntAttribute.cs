namespace NexusTomorrow.Network.Codec;

/// <summary>
/// A two's complement integer member <see cref="Bits"/> wide, sign-extended on read. On an array member it describes each element.
/// </summary>
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
public sealed class SIntAttribute : Attribute
{
    /// <summary>
    /// Declare the member's width, 1 to 64 bits.
    /// </summary>
    public SIntAttribute(int bits)
    {
        Bits = bits;
    }

    /// <summary>
    /// The member's width in bits.
    /// </summary>
    public int Bits { get; }
}
