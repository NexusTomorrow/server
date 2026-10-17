using NexusTomorrow.Network.Codec;

namespace NexusTomorrow.Network.Message.Model;

/// <summary>
/// A client message encrypted for the connection it is sent on. The client sends every message inside one.
/// </summary>
/// <remarks>
/// The message inside is either a <c>TaggedMessage</c> (0x038C), which carries another message under a tag, or a message on its own
/// with no tag. The client sends tagged messages before the world as well as in it. Those it sends on their own include
/// <c>LoginRequest</c> (0x0592), <c>WorldHello</c> (0x058F), <c>CharacterListRequest</c> (0x07E0), <c>CharacterSelect</c> (0x07DD),
/// <c>CharacterDelete</c> (0x0352), <c>PregameKeepAlive</c> (0x0241) and <see cref="NestedMessage"/>.
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
