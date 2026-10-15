namespace NexusTomorrow.Network.Codec;

/// <summary>
/// A <c>byte[]</c> member written as an optional byte alignment, a length <see cref="LengthBits"/> wide, then the bytes.
/// </summary>
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
public sealed class BlobAttribute : Attribute
{
    /// <summary>
    /// Declare the width of the length.
    /// </summary>
    public BlobAttribute(int lengthBits)
    {
        LengthBits = lengthBits;
    }

    /// <summary>
    /// The width of the length, 1 to 32 bits.
    /// </summary>
    public int LengthBits { get; }

    /// <summary>
    /// True to skip to a byte boundary before the length; allowed only in a [WireMessage] type, which starts on one.
    /// </summary>
    public bool Align { get; set; }

    /// <summary>
    /// True when the length counts its own bytes as well as the payload's.
    /// </summary>
    public bool LengthIncludesSelf { get; set; }
}
