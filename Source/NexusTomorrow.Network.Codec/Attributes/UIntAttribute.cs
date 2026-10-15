namespace NexusTomorrow.Network.Codec;

/// <summary>
/// An unsigned integer, bool or enum member <see cref="Bits"/> wide, zero-extended on read. On an array member it describes each element.
/// </summary>
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
public sealed class UIntAttribute : Attribute
{
    /// <summary>
    /// Declare the member's width, 1 to 64 bits.
    /// </summary>
    public UIntAttribute(int bits)
    {
        Bits = bits;
    }

    /// <summary>
    /// The member's width in bits.
    /// </summary>
    public int Bits { get; }
}
