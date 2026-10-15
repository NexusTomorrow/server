namespace NexusTomorrow.Network.Codec;

/// <summary>
/// A float (32 bits) or <see cref="System.Numerics.Vector3"/> (three times 32 bits) member in IEEE 754 single precision.
/// </summary>
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
public sealed class Float32Attribute : Attribute
{
}
