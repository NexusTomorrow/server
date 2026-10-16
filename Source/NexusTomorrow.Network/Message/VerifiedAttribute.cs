namespace NexusTomorrow.Network.Message;

/// <summary>
/// How far a message's declaration has been read against the client. A message with no <see cref="VerifiedAttribute"/> is
/// known only by its opcode, direction and layout, which the client facts hold it to, and keeps neutral names.
/// </summary>
public enum Rung
{
    /// <summary>
    /// What the message is for is read: its type is named for that purpose instead of its neutral opcode name, and carries a summary.
    /// </summary>
    Purpose = 1,

    /// <summary>
    /// Every field is read: each wire member of every layout the message reaches is named for its meaning and carries a summary of
    /// that meaning, unit and range. A member the client gives no meaning keeps its neutral name, and its summary says so first.
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
