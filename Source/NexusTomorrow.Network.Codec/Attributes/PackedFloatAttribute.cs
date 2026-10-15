namespace NexusTomorrow.Network.Codec;

/// <summary>
/// A float (16 bits) or <see cref="System.Numerics.Vector3"/> (three times 16 bits) member in the client's packed float.
/// </summary>
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
public sealed class PackedFloatAttribute : Attribute
{
}
