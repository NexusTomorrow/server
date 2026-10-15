namespace NexusTomorrow.Network.Codec;

/// <summary>
/// Marks a partial class whose wire members, in declaration order, form a layout the generator reads and writes: a nested structure, a union arm or the base of another layout.
/// </summary>
[AttributeUsage(AttributeTargets.Class, Inherited = false)]
public sealed class WireStructAttribute : Attribute
{
}
