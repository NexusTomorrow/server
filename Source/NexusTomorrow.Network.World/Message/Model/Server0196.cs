using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model;

[WireMessage(GameMessageOpcode.Server0196, Direction.ServerToClient)]
public sealed partial class Server0196
{
    [UInt(5)] public byte Unknown0 { get; set; }
    [UInt(1)] public bool Unknown1 { get; set; }
}
