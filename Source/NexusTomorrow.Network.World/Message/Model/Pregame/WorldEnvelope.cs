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
    /// A 32-bit length that counts its own four bytes, then the encrypted message; the length must be at least 4. The client decrypts the
    /// bytes after the length and handles them as one whole message.
    /// </summary>
    [Blob(32, LengthIncludesSelf = true)] public byte[] Payload { get; set; } = [];
}
