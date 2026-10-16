namespace NexusTomorrow.Network.Message;

/// <summary>
/// How far a message's declaration has been read against the client. A message with no <see cref="VerifiedAttribute"/> is
/// known only by its opcode, direction and layout, which the client facts hold it to, and keeps neutral names.
/// </summary>
public enum Rung
{
    /// <summary>
    /// What the message is for is read: its type carries the client's name and a summary.
    /// </summary>
    Purpose = 1,

    /// <summary>
    /// Every field is read: each wire member of every layout the message reaches carries the client's name and a summary of its meaning,
    /// unit and range.
    /// </summary>
    Fields = 2,

    /// <summary>
    /// Every flow the message takes part in is read: when it may be sent, what must come before it, and what the client does in reply.
    /// </summary>
    Conversation = 3,
}

/// <summary>
/// The highest <see cref="Message.Rung"/> a [WireMessage] declaration has reached; the registry verification tests hold every message to it.
/// </summary>
[AttributeUsage(AttributeTargets.Class, Inherited = false)]
public sealed class VerifiedAttribute : Attribute
{
    /// <summary>
    /// Record the rung reached.
    /// </summary>
    public VerifiedAttribute(Rung rung)
    {
        Rung = rung;
    }

    /// <summary>
    /// The rung reached.
    /// </summary>
    public Rung Rung { get; }
}
