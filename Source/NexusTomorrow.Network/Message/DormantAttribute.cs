namespace NexusTomorrow.Network.Message;

/// <summary>
/// The 16042 client registers the message but neither handles nor sends it, so the server neither handles nor sends it either. A dormant
/// message carries no <see cref="VerifiedAttribute"/>, and the registry verification tests count it apart from the messages still to be read.
/// </summary>
[AttributeUsage(AttributeTargets.Class, Inherited = false)]
public sealed class DormantAttribute : Attribute
{
}
