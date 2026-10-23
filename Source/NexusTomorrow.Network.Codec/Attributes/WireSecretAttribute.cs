namespace NexusTomorrow.Network.Codec;

/// <summary>
/// A member that carries a secret: a session key, a login token or a password. It changes nothing on the wire; a tool that records
/// messages leaves out the body of any message with a member so marked.
/// </summary>
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
public sealed class WireSecretAttribute : Attribute;
