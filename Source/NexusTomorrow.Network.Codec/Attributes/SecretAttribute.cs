namespace NexusTomorrow.Network.Codec;

/// <summary>
/// A member that carries a secret, such as a session key. It changes nothing on the wire; a tool that records messages leaves out the
/// body of any message with a member so marked.
/// </summary>
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
public sealed class SecretAttribute : Attribute;
