namespace NexusTomorrow.Network.Codec;

/// <summary>
/// Marks an integer member as the element count of every later array that names it in <see cref="ArrayAttribute.CountFrom"/>; on write it takes their common length.
/// </summary>
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
public sealed class CountAttribute : Attribute
{
}
