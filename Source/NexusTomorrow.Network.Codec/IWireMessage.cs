namespace NexusTomorrow.Network.Codec;

/// <summary>
/// A message: a layout with an opcode and a direction; the generator implements it for every [WireMessage] type.
/// </summary>
public interface IWireMessage : IWireStruct
{
    /// <summary>
    /// The message's opcode.
    /// </summary>
    static abstract ushort Opcode { get; }

    /// <summary>
    /// The direction or directions the message travels.
    /// </summary>
    static abstract Direction Direction { get; }

    /// <summary>
    /// The message's layout on the empty path, which the facts check compares with the client.
    /// </summary>
    static abstract WireLayout Layout { get; }
}
