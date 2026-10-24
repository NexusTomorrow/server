namespace NexusTomorrow.Network.Message;

/// <summary>
/// The 16042 client never acts on the message. It never sends the message and does nothing with it when it arrives, so the server never
/// sends or handles it either. The mark is independent of the message's <see cref="VerifiedAttribute"/> rung, which still states how far
/// its meaning is read, and the registry verification tests count a dormant message apart from the messages still to be read.
/// </summary>
[AttributeUsage(AttributeTargets.Class, Inherited = false)]
public sealed class DormantAttribute : Attribute
{
}
