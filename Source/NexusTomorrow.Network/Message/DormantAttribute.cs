namespace NexusTomorrow.Network.Message;

/// <summary>
/// The 16042 client registers the message but neither handles nor sends it, so the server neither handles nor sends it either. The mark is
/// independent of the message's <see cref="VerifiedAttribute"/> rung, which still states how far its meaning is read, and the registry
/// verification tests count a dormant message apart from the messages still to be read.
/// </summary>
[AttributeUsage(AttributeTargets.Class, Inherited = false)]
public sealed class DormantAttribute : Attribute
{
}
