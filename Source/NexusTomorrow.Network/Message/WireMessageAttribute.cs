using NexusTomorrow.Network.Codec;

namespace NexusTomorrow.Network.Message;

/// <summary>
/// Marks a partial class as a message: a layout with an opcode and the direction it travels, which starts the body and is never nested. The generator implements <see cref="IWireMessage"/> for it.
/// A class carries this or [WireStruct], not both; a message may derive from a [WireStruct] or another message.
/// </summary>
[AttributeUsage(AttributeTargets.Class, Inherited = false)]
public sealed class WireMessageAttribute : Attribute
{
    /// <summary>
    /// Declare the message's opcode and direction.
    /// </summary>
    public WireMessageAttribute(GameMessageOpcode opcode, Direction direction)
    {
        Opcode = opcode;
        Direction = direction;
    }

    /// <summary>
    /// The message's opcode.
    /// </summary>
    public GameMessageOpcode Opcode { get; }

    /// <summary>
    /// The direction or directions the message travels.
    /// </summary>
    public Direction Direction { get; }
}
