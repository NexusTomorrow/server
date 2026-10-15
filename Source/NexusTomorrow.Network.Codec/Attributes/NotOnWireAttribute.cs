namespace NexusTomorrow.Network.Codec;

/// <summary>
/// Excludes a member of a [WireStruct] or [WireMessage] type from the layout.
/// </summary>
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
public sealed class NotOnWireAttribute : Attribute
{
}
