namespace NexusTomorrow.Network.Codec;

/// <summary>
/// A string member: a 16-bit count of UTF-16 code units that includes a zero terminator, then the code units and the terminator.
/// </summary>
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
public sealed class TerminatedWideStringAttribute : Attribute
{
}
