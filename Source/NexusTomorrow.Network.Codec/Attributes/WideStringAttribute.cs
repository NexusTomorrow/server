namespace NexusTomorrow.Network.Codec;

/// <summary>
/// A string member: a 1-bit flag, a 7 or 15-bit length in UTF-16 code units, then the code units.
/// </summary>
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
public sealed class WideStringAttribute : Attribute
{
}
