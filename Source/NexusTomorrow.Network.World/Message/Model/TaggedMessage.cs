using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model;

/// <summary>
/// One whole client message, under a tag that depends on which message it is.
/// </summary>
/// <remarks>
/// The client sends it before the world as well as in it: at character select, a <see cref="Client082D"/> goes out inside one under
/// tag 0x13.
/// </remarks>
[WireMessage(GameMessageOpcode.Client038C, Direction.ClientToServer)]
[Verified(Rung.Fields)]
public sealed partial class TaggedMessage
{
    /// <summary>
    /// The tag: 0x03, 0x0B or 0x13. A message of a given opcode always travels under the same tag.
    /// </summary>
    [UInt(5)] public byte Tag { get; private set; }

    /// <summary>
    /// One whole message, from the next byte boundary: its own 32-bit length (counting those four bytes), then its 16-bit opcode and
    /// its body.
    /// </summary>
    [Blob(32, Align = true, LengthIncludesSelf = true)] public byte[] Payload { get; private set; } = [];
}
