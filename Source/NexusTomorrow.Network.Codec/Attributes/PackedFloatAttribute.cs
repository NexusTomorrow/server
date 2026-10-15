namespace NexusTomorrow.Network.Codec;

/// <summary>
/// A float (16 bits), <see cref="System.Numerics.Vector2"/>, <see cref="System.Numerics.Vector3"/> or <see cref="System.Numerics.Quaternion"/> (16 bits per component, in X, Y, Z, W order) member in the client's packed float.
/// </summary>
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
public sealed class PackedFloatAttribute : Attribute
{
}
