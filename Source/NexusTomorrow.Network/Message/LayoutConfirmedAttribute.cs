namespace NexusTomorrow.Network.Message;

/// <summary>
/// The client takes the message's layout as declared, every member's width and order. It says nothing of what the message or its members
/// mean, which <see cref="VerifiedAttribute"/> carries. The registry verification tests hold a marked message to the client facts and refuse
/// a pinned disagreement on it.
/// </summary>
[AttributeUsage(AttributeTargets.Class, Inherited = false)]
public sealed class LayoutConfirmedAttribute : Attribute
{
}
