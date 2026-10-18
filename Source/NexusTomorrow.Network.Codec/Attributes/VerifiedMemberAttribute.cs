namespace NexusTomorrow.Network.Codec;

/// <summary>
/// Marks a wire member whose meaning is read from two observations and documented in its summary. The server may set a marked member
/// of a message read to Purpose; every unmarked member of such a message goes as its default, which the generated
/// <see cref="IWireStruct.FindUnverifiedValue"/> checks. Only the audit adds or removes the mark.
/// </summary>
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, Inherited = false)]
public sealed class VerifiedMemberAttribute : Attribute
{
}
