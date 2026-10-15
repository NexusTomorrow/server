using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model;

[WireMessage(GameMessageOpcode.ClientPackedWorld, Direction.ClientToServer)]
public sealed partial class ClientPackedWorld
{
    // TODO: research this more
    // client hardcoded value of either 11 or 19 depending on what function sends the packet
    [UInt(5)] public byte Unknown0 { get; private set; }
    [Blob(32, Align = true, LengthIncludesSelf = true)] public byte[] Data { get; private set; } = [];
}
