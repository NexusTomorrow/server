namespace NexusTomorrow.Network.Codec;

/// <summary>
/// A float (32 bits), <see cref="System.Numerics.Vector2"/>, <see cref="System.Numerics.Vector3"/> or <see cref="System.Numerics.Quaternion"/> (32 bits per component, in X, Y, Z, W order) member in IEEE 754 single precision.
/// </summary>
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
public sealed class Float32Attribute : Attribute
{
}
