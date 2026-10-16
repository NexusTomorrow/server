using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model;

[WireMessage(GameMessageOpcode.Client038C, Direction.ClientToServer)]
public sealed partial class Client038C
{
    [UInt(5)] public byte Unknown0 { get; private set; }
    [Blob(32, Align = true, LengthIncludesSelf = true)] public byte[] Unknown1 { get; private set; } = [];
}
