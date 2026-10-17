using NexusTomorrow.Network.Codec;

namespace NexusTomorrow.Network.Message.Model;

/// <summary>
/// A client message encrypted for the connection it is sent on. The client sends every message inside one.
/// </summary>
/// <remarks>
/// In the world, the message inside is <c>TaggedMessage</c> (0x038C), which carries the game message.
/// </remarks>
[WireMessage(GameMessageOpcode.Client0244, Direction.ClientToServer)]
[Verified(Rung.Fields)]
public sealed partial class ClientEnvelope
{
    /// <summary>
    /// One whole message, which begins with its own 32-bit length (counting those four bytes). The length travels in clear and the bytes
    /// after it are encrypted with the key of the connection it is sent on.
    /// </summary>
    [Blob(32, LengthIncludesSelf = true)] public byte[] Payload { get; private set; } = [];
}
