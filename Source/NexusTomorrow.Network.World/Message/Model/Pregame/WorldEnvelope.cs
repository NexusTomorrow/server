using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Pregame;

/// <summary>
/// A server message encrypted with the world connection's key.
/// </summary>
/// <remarks>
/// The client picks the key by opcode, not by the connection the envelope arrives on, and drops a server message that arrives bare on the
/// world connection.
/// </remarks>
[WireMessage(GameMessageOpcode.Server03DC, Direction.ServerToClient)]
[Verified(Rung.Fields)]
public sealed partial class WorldEnvelope
{
    /// <summary>
    /// One whole message, which begins with its own 32-bit length (counting those four bytes). The length travels in clear and the bytes
    /// after it are encrypted with the world connection's key; the client decrypts them and handles the result as that message.
    /// </summary>
    [Blob(32, LengthIncludesSelf = true)] public byte[] Payload { get; set; } = [];
}
