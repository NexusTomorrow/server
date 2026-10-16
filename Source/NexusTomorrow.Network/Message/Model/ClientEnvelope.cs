using NexusTomorrow.Network.Codec;

namespace NexusTomorrow.Network.Message.Model;

/// <summary>
/// A client message encrypted for the connection it is sent on. The client sends every message inside one.
/// </summary>
/// <remarks>
/// In the world, the message inside is <c>Client038C</c> (0x038C), which carries the game message.
/// </remarks>
[WireMessage(GameMessageOpcode.Client0244, Direction.ClientToServer)]
[Verified(Rung.Fields)]
public sealed partial class ClientEnvelope
{
    /// <summary>
    /// A 32-bit length that counts its own four bytes, then the encrypted message. Decrypted with the connection's key, the bytes after the
    /// length are one whole message.
    /// </summary>
    [Blob(32, LengthIncludesSelf = true)] public byte[] Payload { get; private set; } = [];
}
